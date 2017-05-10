using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using RestApi.Models;
using RestApi.Models.Neo4j;

namespace RestApi.Controllers
{
    [Produces("application/json")]
    [Route("api/routes")]
    public class RoutesController : Controller
    {
        private IGraphClient _graphClient;

        public RoutesController(IGraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        /// <summary>
        /// Create relation between bus stop and route number
        /// </summary>
        [HttpPut]
        public IActionResult CreateScheduleEntry([FromBody]ScheduleEntry scheduleEntry)
        {
            var newRoute = new RouteNeo4JModel
            {
                RouteNumber = scheduleEntry.RouteNumber
            };
            var newUnitStop = new UnitStopNeo4JModel
            {
                Id = scheduleEntry.UnitStopId
            };
            var newScheduler = new ScheduleNeo4JModel
            {
                Time = scheduleEntry.Time,
                DayOfWeek = scheduleEntry.DayOfWeek
            };

            // � �� ���������� ��� ������!!!
            // Create route
            _graphClient.Cypher
                .Merge("(route:Route { number : {number} })")
                .OnCreate()
                .Set("route = {newRoute}")
                .WithParams(new
                {
                    number = newRoute.RouteNumber,
                    newRoute
                });

            // Create stop
            _graphClient.Cypher
                .Merge("(stop:STOP { stop_id : {stop_id} })")
                .OnCreate()
                .Set("stop = {newUnitStop}")
                .WithParams(new
                {
                    stop_id = newUnitStop,
                    newUnitStop
                });

            // Create schedule entry
            _graphClient.Cypher
                .Merge("(schedule_entry:SCHEDULE_ENTRY { day : {day}, time: {time} })")
                .OnCreate()
                .Set("schedule_entry = {new_schedule_entry}")
                .WithParams(new
                {
                    day = newScheduler.DayOfWeek,
                    time = newScheduler.Time,
                    new_schedule_entry = newScheduler
                });

            // Create route -> schedule_entry relation
            _graphClient.Cypher
                .Match("(route:Route)", "(schedule_entry:SCHEDULE_ENTRY)")
                .Where((RouteNeo4JModel route) => route.RouteNumber == newRoute.RouteNumber)
                .AndWhere((ScheduleNeo4JModel schedule_entry) => schedule_entry.DayOfWeek == newScheduler.DayOfWeek
                    && schedule_entry.Time == newScheduler.Time)
                .Create("route-[:FRIENDS_WITH]->schedule_entry")
                .ExecuteWithoutResults();

            // Create unit_stop -> schedule_entry relation
            _graphClient.Cypher
                .Match("(unit_stop:Stop)", "(schedule_entry:SCHEDULE_ENTRY)")
                .Where((UnitStopNeo4JModel unit_stop) => unit_stop.Id == newUnitStop.Id)
                .AndWhere((ScheduleNeo4JModel schedule_entry) => schedule_entry.DayOfWeek == newScheduler.DayOfWeek
                                                                  && schedule_entry.Time == newScheduler.Time)
                .Create("unit_stop-[:FRIENDS_WITH]->schedule_entry")
                .ExecuteWithoutResults();

            return Ok();
        }

        /// <summary>
        /// Delete relation between bus stop and route number
        /// </summary>
        [HttpDelete]
        public IActionResult DeleteScheduleEntry([FromBody]ScheduleEntry scheduleEntry)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get relations by unit stop and route id.
        /// </summary>
        [HttpGet("{unitStopId>/{routeNumber}")]
        public IEnumerable<ScheduleEntry> GetRoutes(int unitStopId, string routeNumber)
        {
            // � �� ��� ������
            _graphClient.Cypher
                .Match("(route:ROUTE)-[schedule_entry:SCHEDULE_ENTRY]->(unit_stop:STOP)");
            throw new NotImplementedException();
        }

        [HttpPut]
        public IActionResult UpdateScheduleEntry([FromBody]ScheduleEntry lastItem, [FromBody]ScheduleEntry newItem)
        {
            throw new NotImplementedException();
        }
    }
}