using FlyHigh.DTOs.UserDTOs;

namespace FlyHigh.DTOs.LogInDTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public UserSummaryDto User { get; set; }  // Používám STÁVAJÍCÍ UserSummaryDto
        public List<string> Roles { get; set; }
    }
}
