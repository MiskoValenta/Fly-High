namespace FlyHigh.DTOs.MatchAttendanceDTOs
{
    public class MatchAttendanceSummaryDto
    {
        public int MatchId { get; set; }
        public int AttendingCount { get; set; }
        public int NotAttendingCount { get; set; }
        public int MaybeCount { get; set; }
        public int NoResponseCount { get; set; }
        public List<MatchAttendanceDto> Responses { get; set; } = new();

    }
}
