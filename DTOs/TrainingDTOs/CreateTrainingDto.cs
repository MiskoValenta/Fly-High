using System.ComponentModel.DataAnnotations;

namespace FlyHigh.DTOs.TrainingDto
{
    public class CreateTrainingDto
    {
        [Required]
        public int TeamId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime ScheduledDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [StringLength(200)]
        public string Location { get; set; }

        [StringLength(200)]
        public string LocationAddress { get; set; }

    }
}
