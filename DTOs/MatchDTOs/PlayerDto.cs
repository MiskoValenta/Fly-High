using System.ComponentModel.DataAnnotations;

namespace FlyHigh.DTOs.MatchDTOs
{
    public class PlayerDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(0, 99)]
        public int? JerseyNumber { get; set; }

        [Required]
        public string Position { get; set; } // "Player", "Libero1", "Libero2", "Coach1", "Coach2", "Coach3"
    }
}
