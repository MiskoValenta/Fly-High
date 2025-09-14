namespace FlyHigh.DTOs.UserDTOs
{
    public class UserSummaryDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
