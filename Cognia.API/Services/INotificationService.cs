using Cognia.Shared.Models;

namespace Cognia.API.Services;

public interface INotificationService
{
    Task SendForumNotificationAsync(NotificationMessage message, CancellationToken cancellationToken = default);
    Task SendReplyNotificationAsync(int threadId, string? author, bool isAnonymous, string content, CancellationToken cancellationToken = default);
}
