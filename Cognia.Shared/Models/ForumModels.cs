namespace Cognia.Shared.Models;

public class ForumThread
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = "General";
    public string Author { get; set; } = "Guest";
    public string Content { get; set; } = string.Empty;
    public bool IsAnonymous { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<ForumReply> Replies { get; set; } = new();
}

public class ForumReply
{
    public int Id { get; set; }
    public int ThreadId { get; set; }
    public string Author { get; set; } = "Guest";
    public string Content { get; set; } = string.Empty;
    public bool IsAnonymous { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class CreateForumThreadRequest
{
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = "General";
    public string Author { get; set; } = "Guest";
    public string Content { get; set; } = string.Empty;
    public bool IsAnonymous { get; set; }
}

public class CreateForumReplyRequest
{
    public int ThreadId { get; set; }
    public string Author { get; set; } = "Guest";
    public string Content { get; set; } = string.Empty;
    public bool IsAnonymous { get; set; }
}

public static class ForumCategories
{
    public static readonly string[] All =
    [
        "General",
        "Anxiety",
        "Stress",
        "Sleep",
        "Relationships",
        "Academic Pressure",
        "Self-Care",
        "Recovery"
    ];
}
