using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Cognia.API.Data;
using Cognia.Shared.Models;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace Cognia.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TherapistsController : ControllerBase
{
    private readonly CogniaMongoDatabase _database;

    public TherapistsController(CogniaMongoDatabase database) => _database = database;

    [HttpGet]
    public async Task<ActionResult<List<Therapist>>> GetTherapists([FromQuery] string? specialization, CancellationToken cancellationToken)
    {
        var filter = string.IsNullOrWhiteSpace(specialization)
            ? FilterDefinition<Therapist>.Empty
            : Builders<Therapist>.Filter.Regex(therapist => therapist.Specialization,
                new MongoDB.Bson.BsonRegularExpression(Regex.Escape(specialization), "i"));
        return Ok(await _database.Therapists.Find(filter).ToListAsync(cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Therapist>> GetTherapist(int id, CancellationToken cancellationToken)
    {
        var therapist = await _database.Therapists.Find(item => item.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (therapist == null)
        {
            return NotFound(new { message = $"Therapist with ID {id} not found." });
        }
        return Ok(therapist);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Therapist>> CreateTherapist(Therapist therapist, CancellationToken cancellationToken)
    {
        therapist.Id = await _database.NextIdAsync("therapists", cancellationToken);
        await _database.Therapists.InsertOneAsync(therapist, cancellationToken: cancellationToken);
        return CreatedAtAction(nameof(GetTherapist), new { id = therapist.Id }, therapist);
    }
}
