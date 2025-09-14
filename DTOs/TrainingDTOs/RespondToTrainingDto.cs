using System.ComponentModel.DataAnnotations;

namespace FlyHigh.DTOs.TrainingDto
{
    public class RespondToTrainingDto
    {
        [Required]
        public string Status { get; set; } // Attending, NotAttending, Maybe

        [StringLength(200)]
        public string Note { get; set; }
    }
}
