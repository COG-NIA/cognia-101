namespace Cognia.Shared.Models;

public class NotificationMessage
{
    public string Type { get; set; } = "forum";
    public string Message { get; set; } = string.Empty;
    public string? Author { get; set; }
    public bool IsAnonymous { get; set; }
    public int? ThreadId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
