using System.ComponentModel.DataAnnotations;

namespace FlyHigh.DTOs.NotificationDTOs
{
    public class CreateNotificationDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Message { get; set; }

        [Required]
        public string Type { get; set; }

        public int? RelatedId { get; set; }
    }

    public class NotificationSummaryDto
    {
        public int TotalCount { get; set; }
        public int UnreadCount { get; set; }
        public List<NotificationDto> RecentNotifications { get; set; } = new();

    }
}
