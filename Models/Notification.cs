using System.ComponentModel.DataAnnotations;

namespace FlyHigh.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Message { get; set; }

        [Required]
        public NotificationType Type { get; set; }

        public int? RelatedId { get; set; } // ID souvisejícího objektu (zápas, trénink, etc.)

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ReadAt { get; set; }

        // Navigation properties
        public virtual User User { get; set; }

    }

    public enum NotificationType
    {
        MatchScheduled,    // Naplánován zápas
        MatchResult,       // Výsledek zápasu
        TrainingScheduled, // Naplánován trénink
        TrainingCancelled, // Zrušen trénink
        PollCreated,       // Nová anketa
        TeamInvitation,    // Pozvánka do týmu
        General            // Obecná notifikace
    }
}
