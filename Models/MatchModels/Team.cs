using FlyHigh.Models.TeamModels;
using System.ComponentModel.DataAnnotations;

namespace FlyHigh.Models.MatchModels
{
    public class Team
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = "";

        [StringLength(500)]        
        public string Description { get; set; } = "";

        [StringLength(200)]
        public string LogoUrl { get; set; } = "";

        [StringLength(100)]
        public string City { get; set; } = "";

        [StringLength(20)]
        public string ContactPhone { get; set; } = "";

        [StringLength(100)]
        public string ContactEmail { get; set; } = "";

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual ICollection<TeamMember> Members { get; set; } = new List<TeamMember>();
        public virtual ICollection<Match> HomeMatches { get; set; } = new List<Match>();
        public virtual ICollection<Match> AwayMatches { get; set; } = new List<Match>();
        public virtual ICollection<Training> Trainings { get; set; } = new List<Training>();
        public virtual ICollection<Poll> Polls { get; set; } = new List<Poll>();    
    }
}
