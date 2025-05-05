using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    public class InsertUpdateUserDto 
    {
        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        public string UserName => Email;

        [Required]
        public required string PhoneNumber { get; set; }

        [Required]
        public required AddressDto Address { get; set; }
        [Required]
        public required UserRoles Role { get; set; }
    }
}
