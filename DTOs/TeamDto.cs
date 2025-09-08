namespace FlyHigh.DTOs
{
    public class TeamDto
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string CoachName { get; set; }
        public List<PlayerDto> Players { get; set; }
    }
}
