namespace Core.DTOs
{
    public class BorrowalReturnInfoDto : ResultDto
    {
        public int BorrowalId { get; set; }
        public BorrowalsDisplayDto? BorrowalsDisplayDto { get; set; }

        public string BorrowalDateStr { get; set; } = string.Empty;
        public string DueDateStr { get; set; } = string.Empty;

        public bool IsOverdue { get; set; }
        public string IsOverdueStr => IsOverdue ? "YES" : "no";

        public int LateDays { get; set; }

        public decimal PerDayLateFeeDollars { get; set; }
        public string PerDayLateFeeDollarsStr => $"${PerDayLateFeeDollars}";

        public decimal AmountDue => LateDays * PerDayLateFeeDollars;
        public string AmountDueStr => $"${AmountDue}";
    }
}
