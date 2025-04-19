namespace Core.DTOs
{
    public class AuthorDto : BaseDto
    {
        public required string Name { get; set; }
        public required string Country { get; set; }
        public required string Biography { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string DateOfBirthStr { get; set; } = string.Empty;
    }
}
