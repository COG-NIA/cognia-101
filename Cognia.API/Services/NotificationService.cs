using Cognia.API.Hubs;
using Cognia.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Cognia.API.Services;

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(IHubContext<NotificationHub> hubContext, ILogger<NotificationService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public static string BuildReplyMessage(string? author, bool isAnonymous)
    {
        var trimmedAuthor = string.IsNullOrWhiteSpace(author) ? string.Empty : author.Trim();

        if (isAnonymous || string.IsNullOrWhiteSpace(trimmedAuthor))
        {
            return "Someone";
        }

        return trimmedAuthor;
    }

    public async Task SendReplyNotificationAsync(string? author, bool isAnonymous, string content, CancellationToken cancellationToken = default)
    {
        var authorName = BuildReplyMessage(author, isAnonymous);
        var safeAuthor = isAnonymous ? "Someone" : authorName;

        var message = new NotificationMessage
        {
            Type = "forum-reply",
            Author = safeAuthor,
            IsAnonymous = isAnonymous,
            Message = isAnonymous ? "A new anonymous reply was posted." : $"{safeAuthor} replied to the forum.",
            CreatedAt = DateTime.UtcNow
        };

        await SendForumNotificationAsync(message, cancellationToken);
    }

    public async Task SendForumNotificationAsync(NotificationMessage message, CancellationToken cancellationToken = default)
    {
        try
        {
            await _hubContext.Clients.Group(NotificationHub.ForumGroup).SendAsync("ReceiveNotification", message, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send forum notification");
        }
    }
}
