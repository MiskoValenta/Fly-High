namespace FlyHigh.Models
{
    public class Tournament
    {
        public int TournamentId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public int CreatedBy { get; set; } // CoachId

        public User Coach { get; set; }
        public ICollection<Match> Matches { get; set; }
    }
}
