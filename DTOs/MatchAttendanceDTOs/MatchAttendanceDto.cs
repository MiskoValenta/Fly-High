using FlyHigh.DTOs.UserDTOs;

namespace FlyHigh.DTOs.MatchAttendanceDTOs
{
    public class MatchAttendanceDto
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public DateTime ResponseAt { get; set; }
        public UserSummaryDto User { get; set; }

    }
}
