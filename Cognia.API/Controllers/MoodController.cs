using System.Security.Claims;
using Cognia.API.Data;
using Cognia.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Cognia.API.Controllers;

[ApiController]
[Authorize]
[Route("api/mood")]
public sealed class MoodController : ControllerBase
{
    private readonly CogniaMongoDatabase _database;

    public MoodController(CogniaMongoDatabase database) => _database = database;

    [HttpGet]
    public async Task<ActionResult<List<MoodEntry>>> GetEntries(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        var entries = await _database.MoodEntries.Find(entry => entry.UserId == userId)
            .SortByDescending(entry => entry.Date)
            .ToListAsync(cancellationToken);
        return Ok(entries);
    }

    [HttpPost]
    public async Task<ActionResult<MoodEntry>> CreateEntry(CreateMoodEntryRequest request, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        if (request.Score is < 1 or > 5 || request.Note.Length > 240)
        {
            return BadRequest("Score must be 1-5 and note must be 240 characters or fewer.");
        }

        var entry = new MoodEntry
        {
            Id = await _database.NextIdAsync("moodEntries", cancellationToken),
            UserId = userId,
            Date = DateTime.UtcNow,
            Score = request.Score,
            Note = request.Note.Trim()
        };
        await _database.MoodEntries.InsertOneAsync(entry, cancellationToken: cancellationToken);
        return CreatedAtAction(nameof(GetEntries), entry);
    }

    private bool TryGetUserId(out int userId) =>
        int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
}