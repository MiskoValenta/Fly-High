using FlyHigh.DTOs.UserDTOs;

namespace FlyHigh.DTOs.PollDTOs
{
    public class PollResponseDto
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserSummaryDto User { get; set; } // pouze pokud není anonymní

    }
}
