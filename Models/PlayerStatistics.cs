using FlyHigh.Models.MatchModels;
using System.ComponentModel.DataAnnotations;

namespace FlyHigh.Models
{
    public class PlayerStatistics
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int TeamId { get; set; }

        [Required]
        public int Season { get; set; }

        public int MatchesPlayed { get; set; }
        public int SetsPlayed { get; set; }
        public int Points { get; set; }
        public int Aces { get; set; }      // Esa
        public int Blocks { get; set; }    // Bloky
        public int Attacks { get; set; }   // Útoky
        public int Errors { get; set; }    // Chyby

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Team Team { get; set; }

    }
}
