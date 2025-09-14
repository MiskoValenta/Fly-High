using System.ComponentModel.DataAnnotations;

namespace FlyHigh.Models.TeamModels
{
    public class TrainingAttendance
    {
        public int Id { get; set; }

        [Required]
        public int TrainingId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public AttendanceStatus Status { get; set; }

        [StringLength(200)]
        public string Note { get; set; }

        public DateTime ResponseAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual Training Training { get; set; }
        public virtual User User { get; set; }

    }

    public enum  AttendanceStatus
    {
        Attending,    // Přijde
        NotAttending, // Nepřijde
        Maybe,        // Možná
        NoResponse    // Neodpověděl
    }
}
