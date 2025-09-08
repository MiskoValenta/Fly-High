namespace FlyHigh.Models
{
    public class Anket
    {
        public int AnketId { get; set; }
        public int TeamId { get; set; }
        public string Question { get; set; }
        public string Type { get; set; } // YesNo, MultiChoice

        public Team Team { get; set; }
        public ICollection<AnketVote> Votes { get; set; }
    }
}
