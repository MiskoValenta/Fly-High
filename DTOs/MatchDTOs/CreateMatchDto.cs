using System.ComponentModel.DataAnnotations;

namespace FlyHigh.DTOs.MatchDTOs
{
    public class CreateMatchDto
    {
        [Required]
        [StringLength(100)]
        public string HomeTeam { get; set; }

        [Required]
        [StringLength(100)]
        public string AwayTeam { get; set; }

        [Required]
        public DateTime MatchDate { get; set; }

        [Required]
        public TimeSpan MatchTime { get; set; }

        [StringLength(50)]
        public string MatchIdentifier { get; set; }

        public string Notes { get; set; }

        public List<PlayerDto> TeamAPlayers { get; set; } = new();
        public List<PlayerDto> TeamBPlayers { get; set; } = new();
        public List<SetDto> Sets { get; set; } = new();
        public List<PenaltyDto> Penalties { get; set; } = new();
        public MatchResultDto MatchResult { get; set; }
    }
}
