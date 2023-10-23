using Domain.Common;
using System.Text.Json.Serialization;

namespace Domain.PriceListAggregate
{
    public class PriceList
    {
        public string Id { get; set; }
        public Expires Expires { get; set; }
        public List<Route> Routes { get; set; }
    }

    public class Expires
    {
        public string Date { get; set; }
        public int TimezoneType { get; set; }
        public string Timezone { get; set; }
    }

    public class Route
    {
        public string Id { get; set; }
        public Location From { get; set; }
        public Location To { get; set; }
        public int Distance { get; set; }
        public List<Schedule> Schedule { get; set; }
    }

    public class Location
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

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

    public class StartEnd
    {
        public string Date { get; set; }
        public int TimezoneType { get; set; }
        public string Timezone { get; set; }
    }

    public class Company
    {
        public string Id { get; set; }
        public string State { get; set; }
    }

}
