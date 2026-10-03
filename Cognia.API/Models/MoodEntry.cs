using Cognia.Shared.Models;

namespace Cognia.API.Models;

public class MoodEntry
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }

    public MoodType Mood { get; set; }
    public string? TriggerNotes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
