namespace FlyHigh.DTOs.PollDTOs
{
    public class PollSummaryDto
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalResponses { get; set; }
        public bool HasUserResponded { get; set; }

    }
}
