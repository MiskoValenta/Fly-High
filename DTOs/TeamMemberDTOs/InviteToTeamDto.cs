using System.ComponentModel.DataAnnotations;

namespace FlyHigh.DTOs.TeamMemberDTOs
{
    public class InviteToTeamDto
    {
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        public string Role { get; set; } // Player, Coach, AssistantCoach, etc.

        public int? JerseyNumber { get; set; }

        [StringLength(50)]
        public string Position { get; set; }

    }
}
