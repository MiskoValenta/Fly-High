// Models/MatchModels/Match.cs - KOMPLETNĚ NOVÝ
using System.ComponentModel.DataAnnotations;

namespace FlyHigh.Models.MatchModels
{
    public class Match
    {
        public int Id { get; set; }
        public int? HomeTeamId { get; set; }
        public int? AwayTeamId { get; set; }

        [StringLength(100)]
        public string HomeTeamName { get; set; }

        [StringLength(100)]
        public string AwayTeamName { get; set; }

        public string HomeTeam => HomeTeamEntity?.Name ?? HomeTeamName;
        public string AwayTeam => AwayTeamEntity?.Name ?? AwayTeamName;

        [Required]
        public DateTime MatchDate { get; set; }

        [Required]
        public TimeSpan MatchTime { get; set; }

        [StringLength(50)]
        public string MatchIdentifier { get; set; }

        public string Notes { get; set; }
        public string? CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // ČISTÉ navigation properties
        public virtual Team HomeTeamEntity { get; set; }
        public virtual Team AwayTeamEntity { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual ICollection<Set> Sets { get; set; } = new List<Set>();
        public virtual ICollection<Penalty> Penalties { get; set; } = new List<Penalty>();
        public virtual MatchResult MatchResult { get; set; }
        public virtual ICollection<MatchAttendance> Attendances { get; set; } = new List<MatchAttendance>();
    }
}
