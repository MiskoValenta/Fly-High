using FlyHigh.DTOs.UserDTOs;

namespace FlyHigh.DTOs.PollDTOs
{
    public class PollOptionResultDto
    {
        public PollOptionDto Option { get; set; }
        public int VoteCount { get; set; }
        public double Percentage { get; set; }
        public List<UserSummaryDto> Voters { get; set; } = new(); // pouze pokud není anonymní

    }
}
