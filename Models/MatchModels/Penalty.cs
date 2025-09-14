using System.ComponentModel.DataAnnotations;

namespace FlyHigh.Models.MatchModels
{
    public class Penalty
    {
        public int Id { get; set; }

        [Required]
        public int MatchId { get; set; }

        [Required]
        public PenaltyType Type { get; set; }

        [Required]
        public char Team { get; set; } // 'A' or 'B'

        public string JerseyNumber { get; set; } // Optional

        [Range(1, 5)]
        public int Set { get; set; }

        [StringLength(10)]
        public string ScoreAtTime { get; set; } // e.g., "12-14"

        // Navigation property
        public virtual Match Match { get; set; }
    }
    public enum PenaltyType
    {
        Yellow,
        YellowRed,
        Red
    }
}
