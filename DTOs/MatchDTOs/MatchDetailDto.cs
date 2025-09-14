namespace FlyHigh.DTOs.MatchDTOs
{
    public class MatchDetailDto
    {
        public int Id { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public DateTime MatchDate { get; set; }
        public TimeSpan MatchTime { get; set; }
        public string MatchIdentifier { get; set; }
        public string Notes { get; set; }
        public List<SetDto> Sets { get; set; } = new();
        public List<PlayerDto> TeamAPlayers { get; set; } = new();
        public List<PlayerDto> TeamBPlayers { get; set; } = new();
        public List<PenaltyDto> Penalties { get; set; } = new();
        public MatchResultDto MatchResult { get; set; }
    }
}
