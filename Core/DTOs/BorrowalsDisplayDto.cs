using Core.Enums;

namespace Core.DTOs
{
    public class BorrowalsDisplayDto : BaseDto
    {
        public required DateOnly BorrowalDate { get; set; }
        public string BorrowalDateStr => BorrowalDate.ToShortDateString();

        public required DateOnly DueDate { get; set; }
        public bool? IsDelayed => BorrowalStatus == BorrowalStatus.Returned ? null : BorrowalStatus == BorrowalStatus.Out && DueDate < DateOnly.FromDateTime(DateTime.Today);
        public string DueDateStr => DueDate.ToShortDateString();

        public required BorrowalStatus BorrowalStatus { get; set; }
        public string BorrowalStatusStr => BorrowalStatus.ToString();


        // Foreign Keys
        public required int BookId { get; set; }
        public required string AppUserId { get; set; }


        // FK nav properties to display
        public required BookWithAuthorsDto Book { get; set; }
        public string BookDisplayStr => $"{Book.BookTitle} by {Book.AuthorListStr} - {Book.BookPublishedDateStr}";
        public string BookPictureUrl => $"{Book.BookPictureUrl}";


        public required UserInfoDto AppUser { get; set; }
        public string MemberName => $"{AppUser.FirstName} {AppUser.LastName}";
        public string MemberEmail => $"{AppUser.Email}";
    }

    public class BorrowalsDisplayListDto
    {
        public IReadOnlyList<BorrowalsDisplayDto> BorrowalsList { get; set; } = new List<BorrowalsDisplayDto>();
        public int Count => BorrowalsList.Count;
    }
}
