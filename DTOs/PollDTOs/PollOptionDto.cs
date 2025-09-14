namespace FlyHigh.DTOs.PollDTOs
{
    public class PollOptionDto
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public int ResponseCount { get; set; }
        public double Percentage { get; set; }

    }
}
