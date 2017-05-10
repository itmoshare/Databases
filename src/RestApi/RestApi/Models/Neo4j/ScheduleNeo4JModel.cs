using System;

namespace RestApi.Models.Neo4j
{
    public class ScheduleNeo4JModel
    {
        public TimeSpan Time { get; set; }

        public int DayOfWeek { get; set; }
    }
}
