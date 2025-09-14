using FlyHigh.DTOs.UserDTOs;

namespace FlyHigh.DTOs.TrainingDto
{
    public class TrainingAttendanceDto
    {
        public int Id { get; set; }
        public int TrainingId { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public DateTime ResponseAt { get; set; }
        public UserSummaryDto User { get; set; }
    }
}
