using System.Security.Claims;
using Cognia.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Cognia.Shared.Models;
using Cognia.API.Services;

namespace Cognia.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class PaymentsController : ControllerBase
{
    private readonly CogniaMongoDatabase _database;
    private readonly IPaystackService _paystackService;

    public PaymentsController(CogniaMongoDatabase database, IPaystackService paystackService)
    {
        _database = database;
        _paystackService = paystackService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PaymentRecord>>> GetPaymentHistory(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        var payments = await _database.Payments.Find(payment => payment.UserId == userId.ToString())
            .SortByDescending(payment => payment.CreatedAt)
            .ToListAsync(cancellationToken);
        return Ok(payments);
    }

    [HttpPost("initialize")]
    public async Task<ActionResult<PaystackInitializeResponse>> InitializePayment(
        [FromBody] InitializePaymentRequest request, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        var session = await _database.Sessions.Find(item =>
                item.Id == request.SessionId && item.UserId == userId.ToString())
            .FirstOrDefaultAsync(cancellationToken);
        if (session is null) return NotFound(new { message = "Session not found." });

        var response = await _paystackService.InitializeTransactionAsync(new InitializePaymentRequest
        {
            SessionId = session.Id,
            Email = session.ClientEmail,
            Amount = session.TotalAmount,
            CallbackUrl = request.CallbackUrl
        });
        if (!response.Status || response.Data is null)
            return StatusCode(StatusCodes.Status502BadGateway, response);

        var payment = new PaymentRecord
        {
            Id = await _database.NextIdAsync("payments", cancellationToken),
            SessionId = session.Id,
            UserId = userId.ToString(),
            ClientEmail = session.ClientEmail,
            Amount = session.TotalAmount,
            Currency = "GHS",
            PaymentReference = response.Data.Reference,
            PaymentChannel = "Mobile Money / Card",
            PaymentStatus = "Pending",
            PaystackAccessCode = response.Data.AccessCode,
            CreatedAt = DateTime.UtcNow
        };
        await _database.Payments.InsertOneAsync(payment, cancellationToken: cancellationToken);
        await _database.Sessions.UpdateOneAsync(
            item => item.Id == session.Id,
            Builders<TherapistSession>.Update.Set(item => item.PaymentReference, payment.PaymentReference),
            cancellationToken: cancellationToken);

        return Ok(response);
    }

    [HttpGet("verify/{reference}")]
    public async Task<ActionResult<PaystackVerifyResponse>> VerifyPayment(string reference, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        var payment = await _database.Payments.Find(item =>
                item.PaymentReference == reference && item.UserId == userId.ToString())
            .FirstOrDefaultAsync(cancellationToken);
        if (payment is null) return NotFound(new { message = "Payment not found." });

        var verifyResult = await _paystackService.VerifyTransactionAsync(reference);
        if (verifyResult.Status &&
            verifyResult.Data is { Status: "success" } verifiedData &&
            string.Equals(verifiedData.Reference, payment.PaymentReference, StringComparison.Ordinal) &&
            verifiedData.Amount == payment.Amount * 100)
        {
            await _database.Payments.UpdateOneAsync(
                item => item.Id == payment.Id,
                Builders<PaymentRecord>.Update
                    .Set(item => item.PaymentStatus, "Success")
                    .Set(item => item.PaidAt, DateTime.UtcNow)
                    .Set(item => item.PaymentChannel, verifiedData.Channel),
                cancellationToken: cancellationToken);
            await _database.Sessions.UpdateOneAsync(
                item => item.Id == payment.SessionId && item.UserId == userId.ToString(),
                Builders<TherapistSession>.Update
                    .Set(item => item.Status, "Confirmed")
                    .Set(item => item.MeetingLink, $"https://meet.jit.si/cognia-therapy-{reference[..Math.Min(12, reference.Length)]}"),
                cancellationToken: cancellationToken);
        }
        else
        {
            return Ok(new PaystackVerifyResponse
            {
                Status = false,
                Message = "Paystack did not confirm the expected payment reference and amount."
            });
        }

        return Ok(verifyResult);
    }

    private bool TryGetUserId(out int userId) =>
        int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
}
