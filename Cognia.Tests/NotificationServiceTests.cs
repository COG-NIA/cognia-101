using Cognia.API.Hubs;
using Cognia.API.Services;
using Cognia.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace Cognia.Tests;

public class NotificationServiceTests
{
    [Fact]
    public void BuildReplyMessage_AnonymousOrEmptyAuthor_UsesSomeone()
    {
        Assert.Equal("Someone", NotificationService.BuildReplyMessage("   ", true));
        Assert.Equal("Someone", NotificationService.BuildReplyMessage(null, false));
    }

    [Fact]
    public async Task SendReplyNotificationAsync_DoesNotIncludeReplyTextInBroadcast()
    {
        var clients = new FakeHubClients();
        var service = new NotificationService(new FakeHubContext(clients), NullLogger<NotificationService>.Instance);
        const string secretReply = "The password for the portal is 'hunter2'";

        await service.SendReplyNotificationAsync(7, "Joel Nathan", true, secretReply);

        var message = clients.LastMessage;
        Assert.NotNull(message);
        Assert.Equal(NotificationHub.ForumGroup, clients.LastGroup);
        Assert.Equal("ReceiveNotification", clients.LastMethod);
        Assert.Equal("Someone", message.Author);
        Assert.Equal(7, message.ThreadId);
        Assert.True(message.IsAnonymous);
        Assert.Equal("A new anonymous reply was posted.", message.Message);
        Assert.DoesNotContain(secretReply, message.Message, StringComparison.Ordinal);
    }

    private sealed class FakeHubContext : IHubContext<NotificationHub>
    {
        public FakeHubContext(IHubClients clients)
        {
            Clients = clients;
        }

        public IHubClients Clients { get; }
        public IGroupManager Groups { get; } = new FakeGroupManager();
        public IServiceProvider Services { get; } = new ServiceCollection().BuildServiceProvider();
    }

    private sealed class FakeHubClients : IHubClients
    {
        public NotificationMessage? LastMessage { get; internal set; }
        public string? LastGroup { get; internal set; }
        public string? LastMethod { get; internal set; }

        public IClientProxy All => throw new NotSupportedException();
        public IClientProxy AllExcept(IReadOnlyList<string> excludedConnectionIds) => throw new NotSupportedException();
        public IClientProxy Client(string connectionId) => throw new NotSupportedException();
        public IClientProxy Clients(IReadOnlyList<string> connectionIds) => throw new NotSupportedException();
        public IClientProxy Group(string groupName)
        {
            LastGroup = groupName;
            return new FakeClientProxy(this);
        }
        public IClientProxy GroupExcept(string groupName, IReadOnlyList<string> excludedConnectionIds) => throw new NotSupportedException();
        public IClientProxy Groups(IReadOnlyList<string> groupNames) => throw new NotSupportedException();
        public IClientProxy User(string userId) => throw new NotSupportedException();
        public IClientProxy Users(IReadOnlyList<string> userIds) => throw new NotSupportedException();
    }

    private sealed class FakeClientProxy : IClientProxy
    {
        private readonly FakeHubClients _clients;

        public FakeClientProxy(FakeHubClients clients)
        {
            _clients = clients;
        }

        public Task SendCoreAsync(string method, object?[] args, CancellationToken cancellationToken = default)
        {
            _clients.LastMethod = method;
            _clients.LastMessage = Assert.IsType<NotificationMessage>(args[0]);
            return Task.CompletedTask;
        }
    }

    private sealed class FakeGroupManager : IGroupManager
    {
        public Task AddToGroupAsync(string connectionId, string groupName, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task RemoveFromGroupAsync(string connectionId, string groupName, CancellationToken cancellationToken = default) => Task.CompletedTask;
    }
}
