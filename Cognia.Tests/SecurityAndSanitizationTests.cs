using Cognia.API.Controllers;
using Cognia.API.Data;
using Cognia.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using Xunit;

namespace Cognia.Tests;

public class SecurityAndSanitizationTests
{
    private static ForumController CreateController() => new(
        new CogniaMongoDatabase(new MongoClient("mongodb://localhost:27017").GetDatabase("CogniaTests")))
    {
        ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
    };

    [Fact]
    public async Task CreateThread_ValidInputFromAnonymousCaller_IsRejected()
    {
        var result = await CreateController().CreateThread(new CreateForumThreadRequest
        {
            Title = "A valid topic",
            Content = "Only an authenticated account can publish this."
        }, CancellationToken.None);

        Assert.IsType<UnauthorizedResult>(result.Result);
    }

    [Fact]
    public async Task AddReply_ValidInputFromAnonymousCaller_IsRejected()
    {
        var result = await CreateController().AddReply(1, new CreateForumReplyRequest
        {
            ThreadId = 1,
            Content = "Only an authenticated account can reply."
        }, CancellationToken.None);

        Assert.IsType<UnauthorizedResult>(result.Result);
    }
}
