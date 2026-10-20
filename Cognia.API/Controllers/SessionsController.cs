using System.Security.Claims;
using Cognia.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Cognia.Shared.Models;

namespace Cognia.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class SessionsController : ControllerBase
{
    private readonly CogniaMongoDatabase _database;

    public SessionsController(CogniaMongoDatabase database) => _database = database;

    [HttpGet]
    public async Task<ActionResult<List<TherapistSession>>> GetUserSessions(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        var sessions = await _database.Sessions.Find(session => session.UserId == userId.ToString())
            .SortByDescending(session => session.CreatedAt)
            .ToListAsync(cancellationToken);
        return Ok(sessions);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TherapistSession>> GetSession(int id, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        var session = await _database.Sessions.Find(item => item.Id == id && item.UserId == userId.ToString())
            .FirstOrDefaultAsync(cancellationToken);
        return session is null ? NotFound() : Ok(session);
    }

    [HttpPost]
    public async Task<ActionResult<TherapistSession>> BookSession(BookSessionRequest request, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        var therapist = await _database.Therapists.Find(item => item.Id == request.TherapistId)
            .FirstOrDefaultAsync(cancellationToken);
        if (therapist is null) return BadRequest("Select an available therapist.");
        if (request.SessionDate.Date < DateTime.UtcNow.Date || request.DurationMinutes is < 0 or > 180)
        {
            return BadRequest("Choose a future session date and a duration of up to 180 minutes.");
        }

        var account = await _database.Accounts.Find(item => item.Id == userId).FirstOrDefaultAsync(cancellationToken);
        if (account is null) return Unauthorized();
        var duration = request.DurationMinutes == 0 ? 60 : request.DurationMinutes;

        var session = new TherapistSession
        {
            Id = await _database.NextIdAsync("sessions", cancellationToken),
            UserId = userId.ToString(),
            ClientName = account.DisplayName,
            ClientEmail = account.Email,
            TherapistId = therapist.Id,
            TherapistName = therapist.Name,
            SessionDate = request.SessionDate,
            TimeSlot = request.TimeSlot,
            DurationMinutes = duration,
            SessionMode = string.IsNullOrWhiteSpace(request.SessionMode) ? "Encrypted Video Call" : request.SessionMode,
            TotalAmount = Math.Round(therapist.HourlyRate * duration / 60m, 2),
            Notes = request.Notes,
            Status = "Pending",
            MeetingLink = string.Empty,
            PaymentReference = string.Empty,
            CreatedAt = DateTime.UtcNow
        };

        await _database.Sessions.InsertOneAsync(session, cancellationToken: cancellationToken);
        return CreatedAtAction(nameof(GetSession), new { id = session.Id }, session);
    }

    private bool TryGetUserId(out int userId) =>
        int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
}
