namespace FlyHigh.Models
{
    public class AnketVote
    {
        public int VoteId { get; set; }
        public int AnketId { get; set; }
        public int PlayerId { get; set; }
        public string Answer { get; set; }

        public Anket Anket { get; set; }
        public Player Player { get; set; }
    }
}
