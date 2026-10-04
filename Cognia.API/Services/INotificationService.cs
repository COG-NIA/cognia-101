using Cognia.Shared.Models;

namespace Cognia.API.Services;

public interface INotificationService
{
    Task SendForumNotificationAsync(NotificationMessage message, CancellationToken cancellationToken = default);
    Task SendReplyNotificationAsync(string? author, bool isAnonymous, string content, CancellationToken cancellationToken = default);
}
