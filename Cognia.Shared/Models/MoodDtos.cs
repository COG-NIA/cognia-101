using System.ComponentModel.DataAnnotations;
 
namespace Cognia.Shared.Models;
 
public record MoodEntryResponse(int Id, MoodType Mood, string? TriggerNotes, DateTime CreatedAt);
 
public class CreateMoodEntryRequest
{
    [EnumDataType(typeof(MoodType))]
    public MoodType Mood { get; set; }
 
    [StringLength(1000)]
    public string? TriggerNotes { get; set; }
}
 
public class UpdateMoodEntryRequest
{
    [EnumDataType(typeof(MoodType))]
    public MoodType Mood { get; set; }
 
    [StringLength(1000)]
    public string? TriggerNotes { get; set; }
}
 
/// <summary>Entries per mood for a single day.</summary>
public record MoodTrendPoint(DateOnly Date, IReadOnlyDictionary<string, int> Counts);
 
public record MoodTrendsResponse(
    string Period,
    DateTime From,
    DateTime To,
    int TotalEntries,
    IReadOnlyDictionary<string, int> Totals,
    IReadOnlyList<MoodTrendPoint> Daily);