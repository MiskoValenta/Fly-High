using System.ComponentModel.DataAnnotations;

namespace FlyHigh.DTOs.PollDTOs
{
    public class VotePollDto
    {
        [Required]
        public List<int> SelectedOptionIds { get; set; } = new();

        [StringLength(500)]
        public string Comment { get; set; }

    }
}
