namespace Demo.Domain.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsInsideTimeRange(this DateTime now, DateTime startDate, DateTime endDate)
        {
            return now >= startDate && now <= endDate;
        }
    }
}