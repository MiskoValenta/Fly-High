using FlyHigh.Models;

namespace FlyHigh.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string userId, NotificationType type, string title, string message, int? relatedId = null);
        Task SendTeamNotificationAsync(int teamId, NotificationType type, string title, string message, int? relatedId = null);
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId, bool unreadOnly = false);
        Task MarkAsReadAsync(int notificationId, string userId);
        Task MarkAllAsReadAsync(string userId);
    }

}

