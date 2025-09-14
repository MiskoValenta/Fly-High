using FlyHigh.DTOs.TeamDTOs;

namespace FlyHigh.DTOs.TeamMemberDTOs
{
    public class TeamMembershipDto
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string Role { get; set; }
        public int? JerseyNumber { get; set; }
        public string Position { get; set; }
        public bool IsActive { get; set; }
        public DateTime JoinedAt { get; set; }
        public TeamSummaryDto Team { get; set; }

    }
}
