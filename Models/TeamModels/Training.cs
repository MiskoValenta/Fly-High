using FlyHigh.Models.MatchModels;
using System.ComponentModel.DataAnnotations;

namespace FlyHigh.Models.TeamModels
{
    public class Training
    {
        public int Id { get; set; }

        [Required]
        public int TeamId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime ScheduledDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [StringLength(200)]
        public string Location { get; set; }

        [StringLength(200)]
        public string LocationAddress { get; set; }

        public bool IsCancelled { get; set; } = false;

        [StringLength(300)]
        public string CancellationReason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual Team Team { get; set; }
        public virtual ICollection<TrainingAttendance> Attendances { get; set; } = new List<TrainingAttendance>();

    }
}
