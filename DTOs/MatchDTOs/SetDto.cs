using System.ComponentModel.DataAnnotations;

namespace FlyHigh.DTOs.MatchDTOs
{
    public class SetDto
    {
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
    }
}
