using Cognia.Shared.Models;
using Xunit;

namespace Cognia.Tests;

public class ForumModelTests
{
    [Fact]
    public void ForumThread_DefaultValues_AreCorrectlyInitialized()
    {
        // Act
        var thread = new ForumThread();

        // Assert
        Assert.Equal(string.Empty, thread.Title);
        Assert.Equal("General", thread.Category);
        Assert.Equal("Guest", thread.Author);
        Assert.Equal(string.Empty, thread.Content);
        Assert.False(thread.IsAnonymous);
        Assert.NotNull(thread.Replies);
        Assert.Empty(thread.Replies);
        Assert.True((DateTime.UtcNow - thread.CreatedAt).TotalSeconds < 5);
    }

    [Fact]
    public void ForumReply_DefaultValues_AreCorrectlyInitialized()
    {
        // Act
        var reply = new ForumReply();

        // Assert
        Assert.Equal("Guest", reply.Author);
        Assert.Equal(string.Empty, reply.Content);
        Assert.False(reply.IsAnonymous);
        Assert.True((DateTime.UtcNow - reply.CreatedAt).TotalSeconds < 5);
    }

    [Fact]
    public void ForumCategories_All_ContainsRequiredCategories()
    {
        // Act & Assert
        Assert.Contains("General", ForumCategories.All);
        Assert.Contains("Anxiety", ForumCategories.All);
        Assert.Contains("Stress", ForumCategories.All);
        Assert.Contains("Sleep", ForumCategories.All);
        Assert.Contains("Relationships", ForumCategories.All);
        Assert.Contains("Academic Pressure", ForumCategories.All);
        Assert.Contains("Self-Care", ForumCategories.All);
        Assert.Contains("Recovery", ForumCategories.All);
        Assert.Equal(8, ForumCategories.All.Length);
    }

    [Theory]
    [InlineData("Academic Pressure")]
    [InlineData("Anxiety")]
    [InlineData("General")]
    public void ForumCategories_Includes_StandardCategory(string categoryName)
    {
        Assert.Contains(categoryName, ForumCategories.All);
    }
}
