namespace DestinationDiscoveryService.Helpers
{
    public static class DateHelper
    {
        public static DateTime GetNextFriday(DateTime startDate)
        {
            while (startDate.DayOfWeek != DayOfWeek.Friday)
            {
                startDate = startDate.AddDays(1);
            }
            return startDate;
        }
    }
}
