using System.ComponentModel.DataAnnotations;

namespace FlyHigh.DTOs.MatchDTOs
{
    public class UpdateMatchDto
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
    }
}
