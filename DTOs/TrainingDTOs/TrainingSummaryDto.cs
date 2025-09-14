namespace FlyHigh.DTOs.TrainingDto
{
    public class TrainingSummaryDto
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string Title { get; set; }
        public DateTime ScheduledDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Location { get; set; }
        public bool IsCancelled { get; set; }
        public int AttendingCount { get; set; }
        public int NotAttendingCount { get; set; }
        public int MaybeCount { get; set; }
        public int NoResponseCount { get; set; }

    }
}
