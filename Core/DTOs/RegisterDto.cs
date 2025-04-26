using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public virtual UserRoles Role { get; set; }

        [Required]
        public AddressDto? Address { get; set; }
    }

    public class MemberRegisterDto : RegisterDto
    {
        public MemberRegisterDto()
        {
            Role = UserRoles.Member; // Default value for Role
        }

        // property override
        public override UserRoles Role
        {
            get => base.Role;
            set
            {
                if (value != UserRoles.Member)
                {
                    throw new InvalidOperationException("Role for MemberRegisterDto must always be UserRoles.Member.");
                }
                base.Role = value;
            }
        }
    }
}
