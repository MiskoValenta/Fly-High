using FlyHigh.DTOs.UserDTOs;

namespace FlyHigh.DTOs.StatiscticsDTOs
{
    public class PlayerStatisticsDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TeamId { get; set; }
        public int Season { get; set; }
        public int MatchesPlayed { get; set; }
        public int SetsPlayed { get; set; }
        public int Points { get; set; }
        public int Aces { get; set; }
        public int Blocks { get; set; }
        public int Attacks { get; set; }
        public int Errors { get; set; }
        public DateTime UpdatedAt { get; set; }
        public UserSummaryDto User { get; set; }

    }
}
