using System.ComponentModel.DataAnnotations;

namespace FlyHigh.Models.MatchModels
{
    public class SetPoint
    {
        public int Id { get; set; }

        [Required]
        public int SetId { get; set; }

        [Range(1, 44)]
        public int PointNumber { get; set; }

        public int TeamAScore { get; set; }
        public int TeamBScore { get; set; }

        public char ScoringTeam { get; set; } // 'A' or 'B'

        // Navigation property
        public virtual Set Set { get; set; }
    }
}
