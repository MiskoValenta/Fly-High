using FlyHigh.DTOs.UserDTOs;

namespace FlyHigh.DTOs.PollDTOs
{
    public class PollDetailDto
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string CreatedByUserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool IsMultipleChoice { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserSummaryDto CreatedByUser { get; set; }
        public List<PollOptionDto> Options { get; set; } = new();
        public bool HasUserResponded { get; set; }
        public List<int> UserSelectedOptions { get; set; } = new();

    }
}
