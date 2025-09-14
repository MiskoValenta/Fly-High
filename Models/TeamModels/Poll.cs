using FlyHigh.Models.MatchModels;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace FlyHigh.Models.TeamModels
{
    public class Poll
    {
        public int Id { get; set; }

        [Required]
        public int TeamId { get; set; }

        [Required]
        public string CreatedByUserId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public PollType Type { get; set; }

        public bool IsMultipleChoice { get; set; } = false;

        public bool IsAnonymous { get; set; } = false;

        public DateTime? ExpiresAt { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual Team Team { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual ICollection<PollOption> Options { get; set; } = new List<PollOption>();
        public virtual ICollection<PollResponse> Responses { get; set; } = new List<PollResponse>();
    }

    public enum PollType
    {
        General,        // Obecné hlasování
        MatchAttendance,// Účast na zápase
        TrainingTime,   // Čas tréninku
        TeamEvent,      // Týmová akce
        Equipment       // Vybavení
    }
}
