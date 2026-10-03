using Cognia.API.Controllers;
using Cognia.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Cognia.Tests;

public class SecurityAndSanitizationTests
{
    private readonly ForumController _controller;

    public SecurityAndSanitizationTests()
    {
        _controller = new ForumController();
    }

    [Fact]
    public void CreateThread_TrimsWhitespaceFromTitleAndContent()
    {
        // Arrange
        var request = new CreateForumThreadRequest
        {
            Title = "   Padded Title   ",
            Content = "   Padded content body   ",
            Category = "  Stress  ",
            Author = "  Joel Nathan  "
        };

        // Act
        var result = _controller.CreateThread(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var thread = Assert.IsType<ForumThread>(createdResult.Value);
        Assert.Equal("Padded Title", thread.Title);
        Assert.Equal("Padded content body", thread.Content);
        Assert.Equal("  Stress  ", thread.Category); // Category untouched or custom
        Assert.Equal("  Joel Nathan  ", thread.Author);
    }

    [Fact]
    public void AddReply_TrimsWhitespaceFromReplyContent()
    {
        // Arrange
        var request = new CreateForumReplyRequest
        {
            ThreadId = 1,
            Author = "Tester",
            Content = "   Untrimmed response   "
        };

        // Act
        var result = _controller.AddReply(1, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var reply = Assert.IsType<ForumReply>(okResult.Value);
        Assert.Equal("Untrimmed response", reply.Content);
    }

    [Theory]
    [InlineData("<script>alert('xss')</script>")]
    [InlineData("SELECT * FROM Users;")]
    [InlineData("<b>Bold text</b>")]
    public void CreateThread_AcceptsInputWithoutCrashing(string rawInput)
    {
        // Arrange
        var request = new CreateForumThreadRequest
        {
            Title = "Security Input Test",
            Content = rawInput,
            Author = "QA Security Test"
        };

        // Act
        var result = _controller.CreateThread(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var thread = Assert.IsType<ForumThread>(createdResult.Value);
        Assert.Equal(rawInput, thread.Content);
    }
}
