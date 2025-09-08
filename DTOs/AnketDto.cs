namespace FlyHigh.DTOs
{
    public class AnketDto
    {
        public int AnketId { get; set; }
        public string Question { get; set; }
        public string Type { get; set; }
        public List<string> Answers { get; set; }
    }
}
