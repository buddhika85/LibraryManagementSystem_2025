using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    public class BookBorrowRequestDto
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }

    public class BorrowResultDto: ResultDto
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string BookAuthors { get; set; } = string.Empty;
        public string MemberEmail { get; set; } = string.Empty;
        public string MemberFullName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set;}
    }
}
