namespace FlyHigh.DTOs.MatchDTOs
{
    public class MatchSummaryDto
    {
        public int Id { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public DateTime MatchDate { get; set; }
        public TimeSpan MatchTime { get; set; }
        public string FinalScore { get; set; }
        public string WinnderTeam { get; set; }
    }
}
