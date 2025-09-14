using System.ComponentModel.DataAnnotations;

namespace FlyHigh.DTOs.PollDTOs
{
    public class CreatePollDto
    {
        [Required]
        public int TeamId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public string Type { get; set; }

        public bool IsMultipleChoice { get; set; } = false;

        public bool IsAnonymous { get; set; } = false;

        public DateTime? ExpiresAt { get; set; }

        [Required]
        public List<CreatePollOptionDto> Options { get; set; } = new();

    }
}
