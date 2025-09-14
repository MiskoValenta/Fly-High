using FlyHigh.DTOs.StatiscticsDTOs;
using FlyHigh.DTOs.TeamMemberDTOs;

namespace FlyHigh.DTOs.TeamDTOs
{
    public class TeamDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
        public string City { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public List<TeamMemberDto> Members { get; set; } = new();
        public TeamStatisticsDto CurrentSeasonStats { get; set; }

    }
}
