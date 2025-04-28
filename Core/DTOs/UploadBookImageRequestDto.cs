using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    public class UploadBookImageRequestDto
    {
        [Required]
        public required IFormFile File { get; set; }
    }
}
