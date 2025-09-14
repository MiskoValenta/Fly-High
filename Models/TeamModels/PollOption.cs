using System.ComponentModel.DataAnnotations;

namespace FlyHigh.Models.TeamModels
{
    public class PollOption
    {
        public int Id { get; set; }

        [Required]
        public int PollId { get; set; }

        [Required]
        [StringLength(200)]
        public string Text { get; set; }

        public int Order { get; set; }

        // Navigation properties
        public virtual Poll Poll { get; set; }
        public virtual ICollection<PollResponse> Responses { get; set; } = new List<PollResponse>();

    }
}
