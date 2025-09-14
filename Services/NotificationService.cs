using FlyHigh.Data;
using FlyHigh.Models;
using FlyHigh.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlyHigh.Services
{
    public class NotificationService : INotificationService
    {
        private readonly VolleyballDbContext _context;
        
        public NotificationService(VolleyballDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId, bool unreadOnly = false)
        {
            var query = _context.Notifications
                .Where(n => n.UserId == userId);

            if (unreadOnly)
            {
                query = query.Where(n => !n.IsRead);
            }

            return await query
                .OrderByDescending(n => n.CreatedAt)
                .Take(50) // Omezit na posledních 50
                .ToListAsync();

        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            var unreadNotifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
        }

        public async Task MarkAsReadAsync(int notificationId, string userId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

            if (notification != null && !notification.IsRead)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }

        }

        public async Task SendNotificationAsync(string userId, NotificationType type, string title, string message, int? relatedId = null)
        {
            var notification = new Notification
            {
                UserId = userId,
                Type = type,
                Title = title,
                Message = message,
                RelatedId = relatedId
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // Zde by se mohlo přidat poslání push notifikace, emailu, atd.
        }

        public async Task SendTeamNotificationAsync(int teamId, NotificationType type, string title, string message, int? relatedId = null)
        {
            // Získat všechny aktivní členy týmu
            var teamMembers = await _context.TeamMembers
                .Where(tm => tm.TeamId == teamId && tm.IsActive)
                .Select(tm => tm.UserId)
                .ToListAsync();

            // Poslat notifikaci každému členovi
            var notifications = teamMembers.Select(userId => new Notification
            {
                UserId = userId,
                Type = type,
                Title = title,
                Message = message,
                RelatedId = relatedId
            }).ToList();

            _context.Notifications.AddRange(notifications);
            await _context.SaveChangesAsync();

        }
    }
}
