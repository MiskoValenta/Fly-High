namespace FlyHigh.DTOs.TeamDTOs
{
    public class TeamSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string LogoUrl { get; set; }
        public int MemberCount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
