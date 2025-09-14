using FlyHigh.DTOs.TeamDTOs;

namespace FlyHigh.DTOs.StatiscticsDTOs
{
    public class TeamStatsSummaryDto
    {
        public TeamSummaryDto Team { get; set; }
        public TeamStatisticsDto CurrentSeason { get; set; }
        public List<TeamStatisticsDto> HistoricalStats { get; set; } = new();
        public List<PlayerStatisticsDto> TopPlayers { get; set; } = new();

    }
}
