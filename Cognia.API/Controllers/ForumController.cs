using Cognia.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cognia.API.Controllers;

[ApiController]
[Route("api/forum")]
public class ForumController : ControllerBase
{
    private static readonly List<ForumThread> Threads = new()
    {
        new ForumThread
        {
            Id = 1,
            Title = "How are you handling academic stress this semester?",
            Category = "Academic Pressure",
            Author = "Student Support",
            Content = "I am trying to balance assignments, deadlines, and rest. What has helped you feel more in control when the workload feels overwhelming?",
            CreatedAt = DateTime.UtcNow.AddDays(-2),
            Replies =
            [
                new ForumReply
                {
                    Id = 1,
                    ThreadId = 1,
                    Author = "Maya",
                    Content = "I started using a simple daily plan and blocking off one low-pressure hour each day. It helps me reset instead of feeling behind all the time.",
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new ForumReply
                {
                    Id = 2,
                    ThreadId = 1,
                    Author = "Anonymous",
                    Content = "Taking short walks in between tasks has made a huge difference. I feel calmer and more focused afterwards.",
                    CreatedAt = DateTime.UtcNow.AddHours(-8),
                    IsAnonymous = true
                }
            ]
        },
        new ForumThread
        {
            Id = 2,
            Title = "Sleep routine tips that actually work",
            Category = "Sleep",
            Author = "Night Owl",
            Content = "I keep losing sleep because my mind starts spiraling at night. I would love to hear what bedtime habits others have found useful.",
            CreatedAt = DateTime.UtcNow.AddDays(-5),
            Replies =
            [
                new ForumReply
                {
                    Id = 3,
                    ThreadId = 2,
                    Author = "Patricia",
                    Content = "Lowering the brightness of my screen and putting my phone on a charger outside the bedroom helped me sleep faster.",
                    CreatedAt = DateTime.UtcNow.AddDays(-4)
                }
            ]
        }
    };

    [HttpGet("threads")]
    public ActionResult<IEnumerable<ForumThread>> GetThreads()
    {
        return Ok(Threads.OrderByDescending(t => t.CreatedAt).ToList());
    }

    [HttpGet("categories")]
    public ActionResult<IEnumerable<string>> GetCategories()
    {
        return Ok(ForumCategories.All);
    }

    [HttpGet("threads/{id}")]
    public ActionResult<ForumThread> GetThread(int id)
    {
        var thread = Threads.FirstOrDefault(t => t.Id == id);
        if (thread is null)
        {
            return NotFound();
        }

        return Ok(thread);
    }

    [HttpGet("threads/{id}/replies")]
    public ActionResult<IEnumerable<ForumReply>> GetReplies(int id)
    {
        var thread = Threads.FirstOrDefault(t => t.Id == id);
        if (thread is null)
        {
            return NotFound();
        }

        return Ok(thread.Replies.OrderBy(r => r.CreatedAt));
    }

    [HttpPost("threads")]
    public ActionResult<ForumThread> CreateThread([FromBody] CreateForumThreadRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest("A title and content are required.");
        }

        var thread = new ForumThread
        {
            Id = Threads.Count == 0 ? 1 : Threads.Max(t => t.Id) + 1,
            Title = request.Title.Trim(),
            Category = string.IsNullOrWhiteSpace(request.Category) ? "General" : request.Category,
            Author = string.IsNullOrWhiteSpace(request.Author) ? "Guest" : request.Author,
            Content = request.Content.Trim(),
            IsAnonymous = request.IsAnonymous,
            CreatedAt = DateTime.UtcNow
        };

        Threads.Insert(0, thread);
        return CreatedAtAction(nameof(GetThread), new { id = thread.Id }, thread);
    }

    [HttpPost("threads/{id}/replies")]
    public ActionResult<ForumReply> AddReply(int id, [FromBody] CreateForumReplyRequest request)
    {
        var thread = Threads.FirstOrDefault(t => t.Id == id);
        if (thread is null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest("Reply content is required.");
        }

        var reply = new ForumReply
        {
            Id = thread.Replies.Count == 0 ? 1 : thread.Replies.Max(r => r.Id) + 1,
            ThreadId = id,
            Author = string.IsNullOrWhiteSpace(request.Author) ? "Guest" : request.Author,
            Content = request.Content.Trim(),
            IsAnonymous = request.IsAnonymous,
            CreatedAt = DateTime.UtcNow
        };

        thread.Replies.Add(reply);
        return Ok(reply);
    }
}
