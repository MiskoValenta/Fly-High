using System.ComponentModel.DataAnnotations;

namespace FlyHigh.DTOs.TeamMemberDTOs
{
    public class UpdateTeamMemberDto
    {
        [Required]
        public string Role { get; set; }

        [Range(0, 99)]
        public int? JerseyNumber { get; set; }

        [StringLength(50)]
        public string Position { get; set; }
    }
}
