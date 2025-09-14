using FlyHigh.DTOs.UserDTOs;

namespace FlyHigh.DTOs.TeamMemberDTOs
{
    public class TeamMemberDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TeamId { get; set; }
        public string Role { get; set; }
        public int? JerseyNumber { get; set; }
        public string Position { get; set; }
        public bool IsActive { get; set; }
        public DateTime JoinedAt { get; set; }

        // Navigation property
        public UserSummaryDto User { get; set; }
    }
}
