using System.ComponentModel.DataAnnotations;

namespace FlyHigh.DTOs.MatchDTOs
{
    public class MatchResultDto
    {
        public int TeamAScore { get; set; }
        public int TeamBScore { get; set; }
        public int TeamATotalPoints { get; set; }
        public int TeamBTotalPoints { get; set; }

        [Required]
        public string WinnderTeam { get; set; }

        [StringLength(10)]
        public string FinalScore { get; set; } // e.g., "3-1"

        public int TotalDurationMinutes { get; set; }
    }
}
