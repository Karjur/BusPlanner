using Domain.PriceListAggregate.ValueObjects;

namespace Domain.PriceListAggregate
{
    public class Schedule
    {
        public string Id { get; set; }
        public double Price { get; set; }
        public StartEnd Start { get; set; }
        public StartEnd End { get; set; }
        public Company Company { get; set; }

        public string StartEndToDate(string date)
        {
            DateTime dateTime = DateTime.Parse(date);
            return dateTime.ToString("M-dd");
        }

        public string StartEndToTime(string date)
        {
            DateTime dateTime = DateTime.Parse(date);
            return dateTime.ToString("HH:mm");
        }
    }
}
