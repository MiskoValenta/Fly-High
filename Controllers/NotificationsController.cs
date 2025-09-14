using FlyHigh.DTOs.NotificationDTOs;
using FlyHigh.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlyHigh.Controllers
{
    [ApiController]
    [Route("Notification/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // GET: api/notifications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotifications(
            [FromQuery] bool unreadOnly = false)
        {
            var currentUserId = User.FindFirst("id")?.Value;
            var notifications = await _notificationService.GetUserNotificationsAsync(currentUserId, unreadOnly);

            var notificationDtos = notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                Type = n.Type.ToString(),
                RelatedId = n.RelatedId,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt,
                ReadAt = n.ReadAt
            });

            return Ok(notificationDtos);
        }

        // GET: api/notifications/summary
        [HttpGet("summary")]
        public async Task<ActionResult<NotificationSummaryDto>> GetNotificationsSummary()
        {
            var currentUserId = User.FindFirst("id")?.Value;
            var allNotifications = await _notificationService.GetUserNotificationsAsync(currentUserId);
            var recentNotifications = await _notificationService.GetUserNotificationsAsync(currentUserId);

            var summary = new NotificationSummaryDto
            {
                TotalCount = allNotifications.Count(),
                UnreadCount = allNotifications.Count(n => !n.IsRead),
                RecentNotifications = recentNotifications.Take(5).Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Message = n.Message,
                    Type = n.Type.ToString(),
                    RelatedId = n.RelatedId,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt,
                    ReadAt = n.ReadAt
                }).ToList()
            };

            return Ok(summary);
        }

        // PUT: api/notifications/5/read
        [HttpPut("{id}/read")]
        public async Task<ActionResult> MarkAsRead(int id)
        {
            var currentUserId = User.FindFirst("id")?.Value;
            await _notificationService.MarkAsReadAsync(id, currentUserId);

            return Ok();
        }

        // PUT: api/notifications/read-all
        [HttpPut("read-all")]
        public async Task<ActionResult> MarkAllAsRead()
        {
            var currentUserId = User.FindFirst("id")?.Value;
            await _notificationService.MarkAllAsReadAsync(currentUserId);

            return Ok();
        }

    }
}
