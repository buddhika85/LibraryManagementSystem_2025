using Core.Enums;

namespace Core.DTOs
{
    public class BorrowalSummaryDto
    {
        public int BorrowalsId { get; set; }
        public DateTime BorrowalDate { get; set; }
        public string BorrowalDateStr => BorrowalDate.ToShortDateString();

        public DateTime DueDate { get; set; }
        public string DueDateStr => DueDate.ToShortDateString();

        public BorrowalStatus BorrowalStatus { get; set; }
        public string BorrowalStatusStr => BorrowalStatus.ToString();

        // Payment Information
        public bool WasPaid { get; set; }
        public bool WasOverdue { get; set; }
        public decimal AmountPaid { get; set; }
        public int LateDaysOnPayment { get; set; }
        public string PaymentAcceptedBy { get; set; } = string.Empty;

        // Book Details
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string BookPic { get; set; } = string.Empty;
        public BookGenre BookGenre { get; set; }
        public string BookGenreStr => BookGenre.ToString();


        // Borrower Details
        public string BorrowerEmail { get; set; } = string.Empty;
        public string BorrowerName { get; set; } = string.Empty;
    }

    public class BorrowalSummaryListDto
    {
        public IReadOnlyCollection<BorrowalSummaryDto> BorrowalSummaries { get; set; } = [];
        public int Count => BorrowalSummaries.Count;
    }
}
