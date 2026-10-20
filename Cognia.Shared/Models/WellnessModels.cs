namespace Cognia.Shared.Models;

public sealed class WellnessArticle
{
    public int Id { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public int ReadTimeMinutes { get; set; }
    public string Kind { get; set; } = string.Empty;
    public string ImageClass { get; set; } = string.Empty;
    public string ImageAlt { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public sealed class MoodEntry
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public int Score { get; set; }
    public string Note { get; set; } = string.Empty;
}

public sealed class CreateMoodEntryRequest
{
    public int Score { get; set; }
    public string Note { get; set; } = string.Empty;
}