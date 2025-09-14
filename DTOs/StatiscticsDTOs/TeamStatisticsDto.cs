namespace FlyHigh.DTOs.StatiscticsDTOs
{
    public class TeamStatisticsDto
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int Season { get; set; }
        public int MatchesPlayed { get; set; }
        public int MatchesWon { get; set; }
        public int MatchesLost { get; set; }
        public int SetsWon { get; set; }
        public int SetsLost { get; set; }
        public int PointsScored { get; set; }
        public int PointsLost { get; set; }
        public double WinPercentage { get; set; }
        public double SetRatio { get; set; }
        public double PointRatio { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
