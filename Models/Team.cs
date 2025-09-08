namespace FlyHigh.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int CoachId { get; set; }

        public User Coach { get; set; }
        public ICollection<Player> Players { get; set; }
    }
}
