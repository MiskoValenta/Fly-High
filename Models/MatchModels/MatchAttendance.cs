using FlyHigh.Models.TeamModels;
using System.ComponentModel.DataAnnotations;

namespace FlyHigh.Models.MatchModels
{
    public class MatchAttendance
    {
        public int Id { get; set; }

        [Required]
        public int MatchId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public AttendanceStatus Status { get; set; }

        [StringLength(200)]
        public string Note { get; set; }

        public DateTime ResponseAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual Match Match { get; set; }
        public virtual User User { get; set; }

    }
}
