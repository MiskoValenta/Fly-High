using FlyHigh.Models.MatchModels;
using System.ComponentModel.DataAnnotations;

namespace FlyHigh.Models.TeamModels
{
    public class TeamMember
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = "";

        [Required]
        public int TeamId { get; set; }

        [Required]
        public TeamRole Role { get; set; }

        [Range(0, 99)]
        public int? JerseyNumber { get; set; }

        [StringLength(50)]
        public string Position { get; set; } = ""; // Smečař, Nahrávač, Libero, atd.

        public bool IsActive { get; set; } = true;

        public DateTime JoinedAt { get; set; } = DateTime.Now;
        public DateTime? LeftAt { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Team Team { get; set; }

    }

    public enum TeamRole
    {
        Player,         // Hráč
        Coach,          // Trenér/hlavní coach
        AssistantCoach, // Asistent trenéra
        Captain,        // Kapitán týmu
        Manager,        // Manager týmu
        Admin           // Admin týmu (může spravovat tým)
    }
}
