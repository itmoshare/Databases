using System;

namespace RestApi.Models
{
    public class ScheduleEntry
    {
        public string UnitStopId { get; set; }

        public string RouteNumber { get; set; }

        public TimeSpan Time { get; set; }

        public int DayOfWeek { get; set; }
    }
}
