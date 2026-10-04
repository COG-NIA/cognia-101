using Microsoft.AspNetCore.Mvc;
using Cognia.Shared.Models;
using Cognia.API.Services;

namespace Cognia.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private static readonly List<PaymentRecord> Payments = new();
    private static int _nextPaymentId = 1;

    private readonly IPaystackService _paystackService;

    public PaymentsController(IPaystackService paystackService)
    {
        _paystackService = paystackService;
    }

    [HttpGet]
    public ActionResult<List<PaymentRecord>> GetPaymentHistory()
    {
        return Ok(Payments.OrderByDescending(p => p.CreatedAt).ToList());
    }

    [HttpPost("initialize")]
    public async Task<ActionResult<PaystackInitializeResponse>> InitializePayment([FromBody] InitializePaymentRequest request)
    {
        var session = SessionsController.GetSessionById(request.SessionId);
        if (session == null)
        {
            return NotFound(new { message = $"Session ID {request.SessionId} not found." });
        }

        var response = await _paystackService.InitializeTransactionAsync(request);
        if (response.Status && response.Data != null)
        {
            var paymentRecord = new PaymentRecord
            {
                Id = _nextPaymentId++,
                SessionId = request.SessionId,
                UserId = session.UserId,
                ClientEmail = request.Email,
                Amount = request.Amount,
                Currency = "GHS",
                PaymentReference = response.Data.Reference,
                PaymentChannel = "Mobile Money / Card",
                PaymentStatus = "Pending",
                PaystackAccessCode = response.Data.AccessCode,
                CreatedAt = DateTime.UtcNow
            };

            Payments.Add(paymentRecord);

            // Update session with payment reference
            session.PaymentReference = response.Data.Reference;
            SessionsController.UpdateSession(session);
        }

        return Ok(response);
    }

    [HttpGet("verify/{reference}")]
    public async Task<ActionResult<PaystackVerifyResponse>> VerifyPayment(string reference)
    {
        var verifyResult = await _paystackService.VerifyTransactionAsync(reference);

        var payment = Payments.FirstOrDefault(p => p.PaymentReference.Equals(reference, StringComparison.OrdinalIgnoreCase));
        if (payment != null && verifyResult.Status)
        {
            payment.PaymentStatus = "Success";
            payment.PaidAt = DateTime.UtcNow;
            if (!string.IsNullOrEmpty(verifyResult.Data?.Channel))
            {
                payment.PaymentChannel = verifyResult.Data.Channel;
            }
        }

        // Update corresponding session to Confirmed & Generate Jitsi Video Link
        var allSessions = SessionsController.GetSessionById(payment?.SessionId ?? 0);
        if (allSessions != null)
        {
            allSessions.Status = "Confirmed";
            allSessions.MeetingLink = $"https://meet.jit.si/cognia-therapy-{reference[..Math.Min(12, reference.Length)]}";
            SessionsController.UpdateSession(allSessions);
        }

        return Ok(verifyResult);
    }
}
