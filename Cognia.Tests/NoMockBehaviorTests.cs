using Cognia.API.Controllers;
using Cognia.API.Data;
using Cognia.API.Models;
using Cognia.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Cognia.Tests;

public sealed class NoMockBehaviorTests
{
    [Fact]
    public async Task PaystackInitialization_WithoutSecret_ReturnsFailure()
    {
        var service = CreatePaystackService();

        var result = await service.InitializeTransactionAsync(new());

        Assert.False(result.Status);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task PaystackVerification_WithoutSecret_ReturnsFailure()
    {
        var service = CreatePaystackService();

        var result = await service.VerifyTransactionAsync("reference");

        Assert.False(result.Status);
        Assert.Null(result.Data);
    }

    [Theory]
    [InlineData("not-an-email", "StrongEnoughPassword!1")]
    [InlineData("person@example.com", "short")]
    public async Task Registration_InvalidCredentials_ReturnsBadRequestWithoutDatabaseAccess(
        string email,
        string password)
    {
        var database = new CogniaMongoDatabase(new MongoClient("mongodb://localhost:27017").GetDatabase("CogniaTests"));
        var controller = new AuthController(
            database,
            new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:SigningKey"] = "a-test-signing-key-with-more-than-32-bytes"
            }).Build(),
            new PasswordHasher<UserAccount>());

        var result = await controller.Register(new AccountRequest
        {
            Email = email,
            DisplayName = "Test User",
            Password = password
        }, CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    private static PaystackService CreatePaystackService() => new(
        new HttpClient(),
        new ConfigurationBuilder().Build());
}