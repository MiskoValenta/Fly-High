namespace FlyHigh.DTOs.PollDTOs
{
    public class PollResultDto
    {
        public PollDetailDto Poll { get; set; }
        public List<PollOptionResultDto> Results { get; set; } = new();
        public int TotalVotes { get; set; }
        public List<PollResponseDto> Comments { get; set; } = new();

    }
}
