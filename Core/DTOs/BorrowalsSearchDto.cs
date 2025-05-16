using Core.Enums;

namespace Core.DTOs
{
    public class BorrowalsSearchDto
    {
        public string? BookName { get; set; }
        public List<int>? AuthorIds { get; set; }
        public List<BookGenre>? Genres { get; set; }
        public string? MemberName { get; set; }
        public string? MemberEmail { get; set; }
        public DateTime? BorrowedOn { get; set; }
        public DateTime? DueOn { get; set; }
        public List<BorrowalStatus>? Statuses { get; set; }
        public int? Delayed { get; set; }       
    }
}
