using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    public class ReturnsAcceptDto
    {
        public int BorrowalId { get; set; }
        public bool IsOverdue { get; set; }
        public bool Paid { get; set; }
        public decimal AmountAccepted { get; set; }

        // Accepted user
        [Required]
        public string Email { get; set; } = string.Empty;
    }

}
