using Cognia.Shared.Models;
using Cognia.API.Data;
using Cognia.API.Models;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;

namespace Cognia.API.Controllers;

[ApiController]
[Route("api/forum")]
public class ForumController : ControllerBase
{
    private readonly CogniaMongoDatabase _database;

    public ForumController(CogniaMongoDatabase database) => _database = database;

    [HttpGet("threads")]
    public async Task<ActionResult<IEnumerable<ForumThread>>> GetThreads(CancellationToken cancellationToken)
    {
        var threads = await _database.ForumThreads.Find(FilterDefinition<ForumThread>.Empty)
            .SortByDescending(thread => thread.CreatedAt)
            .ToListAsync(cancellationToken);
        return Ok(threads);
    }

    [HttpGet("categories")]
    public ActionResult<IEnumerable<string>> GetCategories()
    {
        return Ok(ForumCategories.All);
    }

    [HttpGet("threads/{id}")]
    public async Task<ActionResult<ForumThread>> GetThread(int id, CancellationToken cancellationToken)
    {
        var thread = await _database.ForumThreads.Find(item => item.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (thread is null)
        {
            return NotFound();
        }

        return Ok(thread);
    }

    [HttpGet("threads/{id}/replies")]
    public async Task<ActionResult<IEnumerable<ForumReply>>> GetReplies(int id, CancellationToken cancellationToken)
    {
        var thread = await _database.ForumThreads.Find(item => item.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (thread is null)
        {
            return NotFound();
        }

        return Ok(thread.Replies.OrderBy(reply => reply.CreatedAt));
    }

    [HttpPost("threads")]
    [Authorize]
    public async Task<ActionResult<ForumThread>> CreateThread([FromBody] CreateForumThreadRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest("A title and content are required.");
        }

        if (!TryGetUserId(out var userId)) return Unauthorized();
        var account = await _database.Accounts.Find(item => item.Id == userId).FirstOrDefaultAsync(cancellationToken);
        if (account is null) return Unauthorized();

        var thread = new ForumThread
        {
            Id = await _database.NextIdAsync("forumThreads", cancellationToken),
            Title = request.Title.Trim(),
            Category = string.IsNullOrWhiteSpace(request.Category) ? "General" : request.Category,
            Author = account.DisplayName,
            Content = request.Content.Trim(),
            IsAnonymous = request.IsAnonymous,
            CreatedAt = DateTime.UtcNow
        };

        await _database.ForumThreads.InsertOneAsync(thread, cancellationToken: cancellationToken);
        return CreatedAtAction(nameof(GetThread), new { id = thread.Id }, thread);
    }

    [HttpPost("threads/{id}/replies")]
    [Authorize]
    public async Task<ActionResult<ForumReply>> AddReply(int id, [FromBody] CreateForumReplyRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest("Reply content is required.");
        }

        if (!TryGetUserId(out var userId)) return Unauthorized();
        var account = await _database.Accounts.Find(item => item.Id == userId).FirstOrDefaultAsync(cancellationToken);
        if (account is null) return Unauthorized();

        var reply = new ForumReply
        {
            Id = await _database.NextIdAsync("forumReplies", cancellationToken),
            ThreadId = id,
            Author = account.DisplayName,
            Content = request.Content.Trim(),
            IsAnonymous = request.IsAnonymous,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _database.ForumThreads.UpdateOneAsync(
            thread => thread.Id == id,
            Builders<ForumThread>.Update.Push(thread => thread.Replies, reply),
            cancellationToken: cancellationToken);
        if (result.MatchedCount == 0)
        {
            return NotFound();
        }

        return Ok(reply);
    }

    private bool TryGetUserId(out int userId) =>
        int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out userId);
}
