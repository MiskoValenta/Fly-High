using System.ComponentModel.DataAnnotations;

namespace FlyHigh.DTOs.TeamDTOs
{
    public class CreateTeamDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(200)]
        public string LogoUrl { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(20)]
        public string ContactPhone { get; set; }

        [StringLength(100)]
        public string ContactEmail { get; set; }

    }
}
