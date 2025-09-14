using FlyHigh.Models.TeamModels;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FlyHigh.Models
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = "";

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = "";

        public string FullName => $"{FirstName} {LastName}";

        [StringLength(20)]
        public override string? PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        [StringLength(200)]
        public string ProfilePictureUrl { get; set; } = "";

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginAt { get; set; }

        // Navigation properties
        public virtual ICollection<TeamMember> TeamMemberships { get; set; } = new List<TeamMember>();
        public virtual ICollection<PollResponse> PollResponses { get; set; } = new List<PollResponse>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}