namespace FlyHigh.DTOs.TrainingDto
{
    public class TrainingDetailDto
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ScheduledDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Location { get; set; }
        public string LocationAddress { get; set; }
        public bool IsCancelled { get; set; }
        public string CancellationReason { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<TrainingAttendanceDto> Attendances { get; set; } = new();

    }
}
