using Microsoft.AspNetCore.Mvc;
using Cognia.Shared.Models;

namespace Cognia.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SessionsController : ControllerBase
{
    private static readonly List<TherapistSession> Sessions = new();
    private static int _nextSessionId = 1;

    [HttpGet]
    public ActionResult<List<TherapistSession>> GetAllSessions()
    {
        return Ok(Sessions.OrderByDescending(s => s.CreatedAt).ToList());
    }

    [HttpGet("user/{userId}")]
    public ActionResult<List<TherapistSession>> GetUserSessions(string userId)
    {
        var userSessions = Sessions.Where(s => s.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase))
                                   .OrderByDescending(s => s.CreatedAt)
                                   .ToList();
        return Ok(userSessions);
    }

    [HttpGet("{id:int}")]
    public ActionResult<TherapistSession> GetSession(int id)
    {
        var session = Sessions.FirstOrDefault(s => s.Id == id);
        if (session == null)
        {
            return NotFound(new { message = $"Session with ID {id} not found." });
        }
        return Ok(session);
    }

    [HttpPost]
    public ActionResult<TherapistSession> BookSession([FromBody] BookSessionRequest request)
    {
        var duration = request.DurationMinutes > 0 ? request.DurationMinutes : 60;
        var baseRate = GetTherapistRate(request.TherapistId);
        var totalAmount = (baseRate * duration) / 60m;

        var session = new TherapistSession
        {
            Id = _nextSessionId++,
            UserId = "Guest",
            ClientName = string.IsNullOrWhiteSpace(request.ClientName) ? "Anonymous Client" : request.ClientName,
            ClientEmail = string.IsNullOrWhiteSpace(request.ClientEmail) ? "client@cognia.com" : request.ClientEmail,
            TherapistId = request.TherapistId,
            TherapistName = GetTherapistName(request.TherapistId),
            SessionDate = request.SessionDate,
            TimeSlot = request.TimeSlot,
            DurationMinutes = duration,
            SessionMode = string.IsNullOrWhiteSpace(request.SessionMode) ? "Encrypted Video Call" : request.SessionMode,
            TotalAmount = Math.Round(totalAmount, 2),
            Notes = request.Notes,
            Status = "Pending",
            MeetingLink = string.Empty,
            PaymentReference = string.Empty,
            CreatedAt = DateTime.UtcNow
        };

        Sessions.Add(session);
        return CreatedAtAction(nameof(GetSession), new { id = session.Id }, session);
    }

    public static TherapistSession? GetSessionById(int id) => Sessions.FirstOrDefault(s => s.Id == id);
    public static void UpdateSession(TherapistSession updatedSession)
    {
        var index = Sessions.FindIndex(s => s.Id == updatedSession.Id);
        if (index >= 0)
        {
            Sessions[index] = updatedSession;
        }
    }

    private static string GetTherapistName(int id) => id switch
    {
        1 => "Dr. Hannah Wilson",
        2 => "Emmanuel Baidoo, M.Sc.",
        3 => "Dr. Sarah Mensah",
        4 => "Gibson Osei, MA",
        5 => "Andrew Nii Attram, M.Sc.",
        6 => "Dr. Patricia Aryee",
        7 => "Dr. Yaw Dankwa",
        8 => "Afua Mensah, M.Sc.",
        9 => "Grace Lamptey, M.Sc.",
        _ => "Verified Specialist"
    };

    private static decimal GetTherapistRate(int id) => id switch
    {
        1 => 180.00m,
        2 => 140.00m,
        3 => 220.00m,
        4 => 160.00m,
        5 => 150.00m,
        6 => 195.00m,
        7 => 175.00m,
        8 => 135.00m,
        9 => 165.00m,
        _ => 150.00m
    };
}
