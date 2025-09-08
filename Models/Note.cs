namespace FlyHigh.Models
{
    public class Note
    {
        public int NoteId { get; set; }
        public int PlayerId { get; set; }
        public int CoachId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

        public Player Player { get; set; }
        public User Coach { get; set; }
    }
}
