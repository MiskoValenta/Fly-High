using System.ComponentModel.DataAnnotations;

namespace FlyHigh.Models.MatchModels
{
    public class Player
    {
        public int Id { get; set; }

        [Required]
        public int MatchId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(0, 99)]
        public int? JerseyNumber { get; set; }

        [Required]
        public char Team { get; set; } // 'A' or 'B'

        public PlayerPosition Position { get; set; }

        // Navigation property
        public virtual Match Match { get; set; }
    }
    public enum PlayerPosition
    {
        Player,
        Libero1,
        Libero2,
        Coach1,
        Coach2,
        Coach3,
    }
}
