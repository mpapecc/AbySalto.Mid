namespace AbySalto.Mid.Application.Extensions
{
    public static class DateTimeExtensions
    {
        public static TimeSpan GetRestOfDayTimeSpan(this DateTime dateTime)
        {
            return dateTime.AddDays(1) - DateTime.Now;
        }
    }
}
