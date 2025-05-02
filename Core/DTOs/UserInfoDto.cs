using Core.Enums;

namespace Core.DTOs
{
    public class UserInfoDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public AddressDto? Address { get; set; }
        public UserRoles Role { get; set; }
    }
}
