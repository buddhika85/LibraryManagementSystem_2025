using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    public class UserInfoDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public AddressDto? Address { get; set; }
        public UserRoles Role { get; set; }
    }
}
