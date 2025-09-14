using FlyHigh.Models.MatchModels;
using System.ComponentModel.DataAnnotations;

namespace FlyHigh.Models.TeamModels
{
    public class TeamStatistics
    {
        public int Id { get; set; }

        [Required]
        public int TeamId { get; set; }

        [Required]
        public int Season { get; set; } // Ročník/sezóna

        public int MatchesPlayed { get; set; }
        public int MatchesWon { get; set; }
        public int MatchesLost { get; set; }
        public int SetsWon { get; set; }
        public int SetsLost { get; set; }
        public int PointsScored { get; set; }
        public int PointsLost { get; set; }

        // Computed properties
        public double WinPercentage => MatchesPlayed > 0 ? (double)MatchesWon / MatchesPlayed * 100 : 0;
        public double SetRatio => SetsLost > 0 ? (double)SetsWon / SetsLost : SetsWon;
        public double PointRatio => PointsLost > 0 ? (double)PointsScored / PointsLost : PointsScored;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual Team Team { get; set; }

    }
}
