using Cognia.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cognia.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<MoodEntry> MoodEntries => Set<MoodEntry>();
    public DbSet<Article> Articles => Set<Article>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        b.Entity<ApplicationUser>(e => e.Property(u => u.DisplayName).HasMaxLength(100));

        b.Entity<MoodEntry>(e =>
        {
            e.Property(m => m.Mood).HasConversion<string>().HasMaxLength(20);
            e.Property(m => m.TriggerNotes).HasMaxLength(1000);
            e.HasOne(m => m.User)
             .WithMany(u => u.MoodEntries)
             .HasForeignKey(m => m.UserId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(m => new { m.UserId, m.CreatedAt });
        });

        b.Entity<Article>(e =>
        {
            e.Property(a => a.Title).HasMaxLength(200).IsRequired();
            e.Property(a => a.Category).HasMaxLength(100).IsRequired();
            e.Property(a => a.Author).HasMaxLength(100);
            e.HasIndex(a => a.Category);
        });
    }
}
