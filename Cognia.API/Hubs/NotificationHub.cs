using Microsoft.AspNetCore.SignalR;

namespace Cognia.API.Hubs;

public class NotificationHub : Hub
{
    public const string ForumGroup = "forum";

    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, ForumGroup);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, ForumGroup);
        await base.OnDisconnectedAsync(exception);
    }
}
