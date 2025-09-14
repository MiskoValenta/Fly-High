using System.ComponentModel.DataAnnotations;

namespace FlyHigh.DTOs.PollDTOs
{
    public class CreatePollOptionDto
    {
        [Required]
        [StringLength(200)]
        public string Text { get; set; }

        public int Order { get; set; }

    }
}
