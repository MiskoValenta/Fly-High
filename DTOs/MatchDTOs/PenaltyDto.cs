using System.ComponentModel.DataAnnotations;

namespace FlyHigh.DTOs.MatchDTOs
{
    public class PenaltyDto
    {
        [Required]
        public string Type { get; set; } // "YellowCard", "RedCard", "PenaltyPoint"

        [Required]
        public string Team { get; set; } // "A" or "B"

        public string JerseyNumber { get; set; } // Optional

        [Range(1, 5)]
        public int Set { get; set; }

        [StringLength(10)]
        public string ScoreAtTime { get; set; } // e.g., "12-14"
    }
}
