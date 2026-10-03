using Cognia.API.Controllers;
using Cognia.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Cognia.Tests;

public class ForumControllerTests
{
    private readonly ForumController _controller;

    public ForumControllerTests()
    {
        _controller = new ForumController();
    }

    [Fact]
    public void GetThreads_ReturnsOkResult_WithListOfThreads()
    {
        // Act
        var result = _controller.GetThreads();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var threads = Assert.IsAssignableFrom<IEnumerable<ForumThread>>(okResult.Value);
        Assert.NotEmpty(threads);
    }

    [Fact]
    public void GetCategories_ReturnsAllPredefinedCategories()
    {
        // Act
        var result = _controller.GetCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var categories = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
        Assert.Equal(ForumCategories.All, categories);
    }

    [Fact]
    public void GetThread_ExistingId_ReturnsOkWithThread()
    {
        // Act
        var result = _controller.GetThread(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var thread = Assert.IsType<ForumThread>(okResult.Value);
        Assert.Equal(1, thread.Id);
    }

    [Fact]
    public void GetThread_NonExistingId_ReturnsNotFound()
    {
        // Act
        var result = _controller.GetThread(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void GetReplies_ExistingThread_ReturnsRepliesInOrder()
    {
        // Act
        var result = _controller.GetReplies(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var replies = Assert.IsAssignableFrom<IEnumerable<ForumReply>>(okResult.Value);
        Assert.NotEmpty(replies);
    }

    [Fact]
    public void GetReplies_NonExistingThread_ReturnsNotFound()
    {
        // Act
        var result = _controller.GetReplies(9999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void CreateThread_ValidRequest_ReturnsCreatedAtAction()
    {
        // Arrange
        var request = new CreateForumThreadRequest
        {
            Title = "Managing Anxiety Before Exams",
            Category = "Academic Pressure",
            Author = "Joel Nathan",
            Content = "What are the best strategies to stay calm before a major examination?",
            IsAnonymous = false
        };

        // Act
        var result = _controller.CreateThread(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var thread = Assert.IsType<ForumThread>(createdResult.Value);
        Assert.Equal("Managing Anxiety Before Exams", thread.Title);
        Assert.Equal("Joel Nathan", thread.Author);
        Assert.False(thread.IsAnonymous);
        Assert.True(thread.Id > 0);
    }

    [Fact]
    public void CreateThread_MissingTitle_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateForumThreadRequest
        {
            Title = "",
            Content = "Valid content body",
            Author = "Joel Nathan"
        };

        // Act
        var result = _controller.CreateThread(request);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("A title and content are required.", badRequest.Value);
    }

    [Fact]
    public void CreateThread_MissingContent_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateForumThreadRequest
        {
            Title = "Valid Title",
            Content = "   ",
            Author = "Joel Nathan"
        };

        // Act
        var result = _controller.CreateThread(request);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("A title and content are required.", badRequest.Value);
    }

    [Fact]
    public void CreateThread_AnonymousPosting_SetsAuthorDefaults()
    {
        // Arrange
        var request = new CreateForumThreadRequest
        {
            Title = "Anonymous Wellness Support",
            Category = "Self-Care",
            Author = "",
            Content = "Posting anonymously for privacy.",
            IsAnonymous = true
        };

        // Act
        var result = _controller.CreateThread(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var thread = Assert.IsType<ForumThread>(createdResult.Value);
        Assert.Equal("Guest", thread.Author);
        Assert.True(thread.IsAnonymous);
    }

    [Fact]
    public void AddReply_ValidRequest_ReturnsOkWithReply()
    {
        // Arrange
        var request = new CreateForumReplyRequest
        {
            ThreadId = 1,
            Author = "QA Lead",
            Content = "Try deep breathing exercises 5 minutes before the exam.",
            IsAnonymous = false
        };

        // Act
        var result = _controller.AddReply(1, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var reply = Assert.IsType<ForumReply>(okResult.Value);
        Assert.Equal(1, reply.ThreadId);
        Assert.Equal("QA Lead", reply.Author);
    }

    [Fact]
    public void AddReply_NonExistingThread_ReturnsNotFound()
    {
        // Arrange
        var request = new CreateForumReplyRequest
        {
            ThreadId = 9999,
            Content = "Reply to missing thread"
        };

        // Act
        var result = _controller.AddReply(9999, request);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void AddReply_EmptyContent_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateForumReplyRequest
        {
            ThreadId = 1,
            Content = "  "
        };

        // Act
        var result = _controller.AddReply(1, request);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Reply content is required.", badRequest.Value);
    }
}
