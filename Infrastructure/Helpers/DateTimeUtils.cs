namespace Infrastructure.Helpers
{
    public static class DateTimeUtils
    {
        public static int GetDayGap(DateOnly value)
        {
            return (DateTime.Today - value.ToDateTime(TimeOnly.MinValue)).Days;
        }
    }
}
