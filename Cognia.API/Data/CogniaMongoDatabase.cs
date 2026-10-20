using Cognia.API.Models;
using Cognia.Shared.Models;
using MongoDB.Driver;

namespace Cognia.API.Data;

public sealed class CogniaMongoDatabase
{
    private readonly IMongoCollection<Sequence> _sequences;

    public CogniaMongoDatabase(IMongoDatabase database)
    {
        Accounts = database.GetCollection<UserAccount>("accounts");
        Articles = database.GetCollection<WellnessArticle>("articles");
        ForumThreads = database.GetCollection<ForumThread>("forumThreads");
        MoodEntries = database.GetCollection<MoodEntry>("moodEntries");
        Payments = database.GetCollection<PaymentRecord>("payments");
        Sessions = database.GetCollection<TherapistSession>("sessions");
        Therapists = database.GetCollection<Therapist>("therapists");
        _sequences = database.GetCollection<Sequence>("sequences");
    }

    public IMongoCollection<UserAccount> Accounts { get; }
    public IMongoCollection<WellnessArticle> Articles { get; }
    public IMongoCollection<ForumThread> ForumThreads { get; }
    public IMongoCollection<MoodEntry> MoodEntries { get; }
    public IMongoCollection<PaymentRecord> Payments { get; }
    public IMongoCollection<TherapistSession> Sessions { get; }
    public IMongoCollection<Therapist> Therapists { get; }

    public async Task<int> NextIdAsync(string collectionName, CancellationToken cancellationToken = default)
    {
        var sequence = await _sequences.FindOneAndUpdateAsync(
            item => item.Id == collectionName,
            Builders<Sequence>.Update.Inc(item => item.Value, 1),
            new FindOneAndUpdateOptions<Sequence> { IsUpsert = true, ReturnDocument = ReturnDocument.After },
            cancellationToken);

        return sequence.Value;
    }

    public Task EnsureIndexesAsync(CancellationToken cancellationToken = default)
    {
        var emailIndex = new CreateIndexModel<UserAccount>(
            Builders<UserAccount>.IndexKeys.Ascending(account => account.NormalizedEmail),
            new CreateIndexOptions { Unique = true });
        return Accounts.Indexes.CreateOneAsync(emailIndex, cancellationToken: cancellationToken);
    }

    private sealed class Sequence
    {
        public string Id { get; set; } = string.Empty;
        public int Value { get; set; }
    }
}