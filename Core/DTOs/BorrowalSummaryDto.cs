namespace Core.DTOs
{
    public class BorrowalSummaryDto
    {
        public int BorrowalsId { get; set; }
        public DateTime BorrowalDate { get; set; }
        public DateTime DueDate { get; set; }
        public string BorrowalStatus { get; set; } = string.Empty;

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
        public string BookGenre { get; set; } = string.Empty;


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
