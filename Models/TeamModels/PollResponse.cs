using System.ComponentModel.DataAnnotations;

namespace FlyHigh.Models.TeamModels
{
    public class PollResponse
    {
        public int Id { get; set; }

        [Required]
        public int PollId { get; set; }

        [Required]
        public int PollOptionId { get; set; }

        [Required]
        public string UserId { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual Poll Poll { get; set; }
        public virtual PollOption PollOption { get; set; }
        public virtual User User { get; set; }

    }
}
