namespace FlyHigh.Models
{
    public class Match
    {
        public int MatchId { get; set; }
        public int TournamentId { get; set; }
        public int Team1Id { get; set; }
        public int Team2Id { get; set; }
        public string Score { get; set; } // "3:2"
        public string Referee { get; set; }
        public string Notes { get; set; } // timeouts, substitutions

        public Tournament Tournament { get; set; }
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
    }
}
