using System.ComponentModel.DataAnnotations;

namespace FlyHigh.DTOs.UserDTOs
{
    public class UpdateUserDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        [StringLength(200)]
        public string ProfilePictureUrl { get; set; }

    }
}
