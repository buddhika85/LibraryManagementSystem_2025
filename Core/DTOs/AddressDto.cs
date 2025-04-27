using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    public class AddressDto
    {
        [Required]
        public string Line1 { get; set; } = string.Empty;

        public string? Line2 { get; set; }

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

        [Required]
        public string Postcode { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;

        public override string ToString() => $"{Line1}, {Line2}, {City}, {State}, {Postcode}, {Country}"; 
    }
}
