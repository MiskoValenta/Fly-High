namespace FlyHigh.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public int Height { get; set; }
        public string Position { get; set; } // Setter, Blocker, Libero...
        public string ProfilePictureUrl { get; set; }

        public Team Team { get; set; }
        public ICollection<Note> Notes { get; set; }
    }
}
