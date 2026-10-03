using Cognia.API.Data;
using Cognia.API.Models;
using Cognia.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Cognia.API.Controllers;

/// <summary>Log and review the signed-in user's mood entries.</summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class MoodController(ApplicationDbContext db) : ControllerBase
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new InvalidOperationException("Authenticated user has no id claim.");

    private static MoodEntryResponse ToResponse(MoodEntry m) =>
        new(m.Id, m.Mood, m.TriggerNotes, m.CreatedAt);

    /// <summary>List your mood entries, newest first. Optional date range and mood filter.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<MoodEntryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<MoodEntryResponse>>> GetAll(
        DateTime? from, DateTime? to, MoodType? mood, int page = 1, int pageSize = 20)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = db.MoodEntries.AsNoTracking().Where(m => m.UserId == UserId);
        if (from.HasValue) query = query.Where(m => m.CreatedAt >= from);
        if (to.HasValue) query = query.Where(m => m.CreatedAt <= to);
        if (mood.HasValue) query = query.Where(m => m.Mood == mood);

        var total = await query.CountAsync();
        var items = await query.OrderByDescending(m => m.CreatedAt)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(m => new MoodEntryResponse(m.Id, m.Mood, m.TriggerNotes, m.CreatedAt))
            .ToListAsync();

        return Ok(new PagedResult<MoodEntryResponse>(items, page, pageSize, total));
    }

    /// <summary>Mood trends for the dashboard charts: counts per mood, overall and per day.</summary>
    /// <param name="period">"week" (last 7 days, default) or "month" (last 30 days).</param>
    [HttpGet("trends")]
    [ProducesResponseType(typeof(MoodTrendsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MoodTrendsResponse>> GetTrends(string period = "week")
    {
        var days = period.ToLowerInvariant() switch
        {
            "week" => 7,
            "month" => 30,
            _ => 0
        };
        if (days == 0)
        {
            ModelState.AddModelError(nameof(period), "Period must be 'week' or 'month'.");
            return ValidationProblem(ModelState);
        }

        var to = DateTime.UtcNow;
        var from = to.Date.AddDays(-(days - 1));

        var entries = await db.MoodEntries.AsNoTracking()
            .Where(m => m.UserId == UserId && m.CreatedAt >= from && m.CreatedAt <= to)
            .Select(m => new { m.Mood, m.CreatedAt })
            .ToListAsync();

        var moodNames = Enum.GetNames<MoodType>();
        Dictionary<string, int> Empty() => moodNames.ToDictionary(n => n, _ => 0);

        var totals = Empty();
        var byDay = Enumerable.Range(0, days)
            .ToDictionary(i => DateOnly.FromDateTime(from.AddDays(i)), _ => Empty());

        foreach (var e in entries)
        {
            var name = e.Mood.ToString();
            totals[name]++;
            byDay[DateOnly.FromDateTime(e.CreatedAt)][name]++;
        }

        var daily = byDay.OrderBy(kv => kv.Key)
            .Select(kv => new MoodTrendPoint(kv.Key, kv.Value))
            .ToList();

        return Ok(new MoodTrendsResponse(period.ToLowerInvariant(), from, to, entries.Count, totals, daily));
    }

    /// <summary>Get one of your mood entries.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MoodEntryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MoodEntryResponse>> GetById(int id)
    {
        var entry = await db.MoodEntries.AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id && m.UserId == UserId);
        return entry is null ? NotFound() : Ok(ToResponse(entry));
    }

    /// <summary>Log a new mood entry.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(MoodEntryResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MoodEntryResponse>> Create(CreateMoodEntryRequest req)
    {
        var entry = new MoodEntry { UserId = UserId, Mood = req.Mood, TriggerNotes = req.TriggerNotes };
        db.MoodEntries.Add(entry);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entry.Id }, ToResponse(entry));
    }

    /// <summary>Update one of your mood entries.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(MoodEntryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MoodEntryResponse>> Update(int id, UpdateMoodEntryRequest req)
    {
        var entry = await db.MoodEntries.FirstOrDefaultAsync(m => m.Id == id && m.UserId == UserId);
        if (entry is null) return NotFound();

        entry.Mood = req.Mood;
        entry.TriggerNotes = req.TriggerNotes;
        await db.SaveChangesAsync();

        return Ok(ToResponse(entry));
    }

    /// <summary>Delete one of your mood entries.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var entry = await db.MoodEntries.FirstOrDefaultAsync(m => m.Id == id && m.UserId == UserId);
        if (entry is null) return NotFound();

        db.MoodEntries.Remove(entry);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
