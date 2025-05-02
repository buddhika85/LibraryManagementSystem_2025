using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    public class ChangePasswordDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string OldPassword { get; set; } = string.Empty;
        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}
