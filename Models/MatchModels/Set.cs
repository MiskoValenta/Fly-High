using System.ComponentModel.DataAnnotations;

namespace FlyHigh.Models.MatchModels
{
    public class Set
    {
        public int Id { get; set; }

        [Required]
        public int MatchId { get; set; }

        [Required]
        [Range(1, 5)]
        public int SetNumber { get; set; }

        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        [Range(0, 50)]
        public int TeamAScore { get; set; }

        [Range(0, 50)]
        public int TeamBScore { get; set; }

        public int? DurationMinutes { get; set; }

        // Time-outs
        public string TeamATimeouts { get; set; } // JSON string for saving time-outs
        public string TeamBTimeouts { get; set; }

        // Navigation property
        public virtual Match Match { get; set; }
        public virtual ICollection<SetPoint> Points { get; set; } = new List<SetPoint>();
    }
}
