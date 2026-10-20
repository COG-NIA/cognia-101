using Cognia.API.Controllers;
using Cognia.API.Data;
using Cognia.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using Xunit;

namespace Cognia.Tests;

public class ForumControllerTests
{
    private static ForumController CreateController() => new(
        new CogniaMongoDatabase(new MongoClient("mongodb://localhost:27017").GetDatabase("CogniaTests")))
    {
        ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
    };

    [Fact]
    public void GetThreads_ReturnsOkResult_WithListOfThreads()
    {
        // Act
        var result = CreateController().GetCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var categories = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
        Assert.Equal(ForumCategories.All, categories);
    }

    [Fact]
    public async Task CreateThread_ValidRequest_RequiresAuthenticatedUser()
    {
        var controller = CreateController();
        var result = await controller.CreateThread(new CreateForumThreadRequest
        {
            Title = "A real conversation",
            Content = "A post from a signed-out user must not be stored."
        }, CancellationToken.None);

        Assert.IsType<UnauthorizedResult>(result.Result);
    }

    [Fact]
    public async Task CreateThread_MissingTitle_ReturnsBadRequest()
    {
        var result = await CreateController().CreateThread(new CreateForumThreadRequest
        {
            Title = "",
            Content = "Content"
        }, CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task AddReply_ValidRequest_RequiresAuthenticatedUser()
    {
        var result = await CreateController().AddReply(1, new CreateForumReplyRequest
        {
            Content = "A reply from a signed-out user must not be stored."
        }, CancellationToken.None);

        Assert.IsType<UnauthorizedResult>(result.Result);
    }
}
