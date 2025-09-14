using System.ComponentModel.DataAnnotations;

namespace FlyHigh.Models.MatchModels
{
    public class MatchResult
    {
        public int Id { get; set; }

        [Required]
        public int MatchId { get; set; }

        public int TeamAScore { get; set; }
        public int TeamBScore { get; set; }
        public int TeamATotalPoints { get; set; }
        public int TeamBTotalPoints { get; set; }

        [Required]
        public char WinnerTeam { get; set; } // 'A' or 'B'

        [StringLength(10)]
        public string FinalScore { get; set; } // e.g., "3-1"

        public int TotalDurationMinutes { get; set; }

        // Navigation property
        public virtual Match Match { get; set; }
    }
}
