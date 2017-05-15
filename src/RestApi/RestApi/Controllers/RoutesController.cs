using System;
using System.Linq;
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
                id = scheduleEntry.RouteNumber
            };
            var newUnitStop = new UnitStopNeo4JModel
            {
                id = scheduleEntry.UnitStopId
            };
            var newScheduler = new ScheduleNeo4JModel
            {
                time = scheduleEntry.Time.ToString(),
                day = scheduleEntry.DayOfWeek
            };

            // Create route
            _graphClient.Cypher
                .Merge("(route:Route { number : {number} })")
                .OnCreate()
                .Set("route = {newRoute}")
                .WithParams(new
                {
                    number = newRoute.id,
                    newRoute
                });

            // Create stop
            _graphClient.Cypher
                .Merge("(stop:Stop { stop_id : {stop_id} })")
                .OnCreate()
                .Set("stop = {newUnitStop}")
                .WithParams(new
                {
                    stop_id = newUnitStop,
                    newUnitStop
                });

            // Create schedule entry
            _graphClient.Cypher
                .Merge("(schedule_entry:Timing { day : {day}, time: {time} })")
                .OnCreate()
                .Set("schedule_entry = {new_schedule_entry}")
                .WithParams(new
                {
                    day = newScheduler.day,
                    time = newScheduler.time,
                    new_schedule_entry = newScheduler
                });

            // Create route -> schedule_entry relation
            _graphClient.Cypher
                .Match("(route:Route)", "(schedule_entry:Timing)")
                .Where((RouteNeo4JModel route) => route.id == newRoute.id)
                .AndWhere((ScheduleNeo4JModel schedule_entry) => schedule_entry.day == newScheduler.day
                    && schedule_entry.time == newScheduler.time)
                .Create("route-[:STOPS_AT_TIME]->schedule_entry")
                .ExecuteWithoutResults();

            // Create unit_stop -> schedule_entry relation
            _graphClient.Cypher
                .Match("(unit_stop:Stop)", "(schedule_entry:Timing)")
                .Where((UnitStopNeo4JModel unit_stop) => unit_stop.id == newUnitStop.id)
                .AndWhere((ScheduleNeo4JModel schedule_entry) => schedule_entry.day == newScheduler.day
                                                                  && schedule_entry.time == newScheduler.time)
                .Create("unit_stop-[:APPLIES_TO_STOP]->schedule_entry")
                .ExecuteWithoutResults();

            return Ok();
        }

        /// <summary>
        /// Delete relation between bus stop and route number
        /// </summary>
        [HttpDelete]
        public IActionResult DeleteScheduleEntry([FromBody]ScheduleEntry scheduleEntry)
        {
            _graphClient.Cypher
                .Match("(route:Route)-[:STOPS_AT_TIME]->(timing:Timing)-[:APPLIES_TO_STOP]->(stop:Stop)")
                .Where((RouteNeo4JModel route, UnitStopNeo4JModel stop) =>
                    route.id == scheduleEntry.RouteNumber && stop.id == scheduleEntry.UnitStopId)
                .Delete("timing")
                .ExecuteWithoutResults();
            return Ok();
        }
        
        /// <summary>
        /// Get relations by unit stop and route id.
        /// </summary>
        [HttpGet("{unitStopId}/{routeNumber}")]
        public IEnumerable<ScheduleEntry> GetTimingsForStopAndRoute(string unitStopId, string routeNumber)
        {
            var timings = _graphClient.Cypher
                .Match("(route:Route)-[:STOPS_AT_TIME]->(timing:Timing)-[:APPLIES_TO_STOP]->(stop:Stop)")
                .Where((RouteNeo4JModel route, UnitStopNeo4JModel stop) => 
                route.id == routeNumber && stop.id == unitStopId)
                .Return(timing => timing.As<ScheduleNeo4JModel>());
            var result = timings.Results.Select(snm => new ScheduleEntry
            {
                UnitStopId = unitStopId,
                DayOfWeek = snm.day,
                RouteNumber = routeNumber,
                Time = TimeSpan.Parse(snm.time)
            });
            return result;
        }
        [HttpGet("routeId")]
        public IEnumerable<object> GetRouteWorkingHours(string routeId)
        {

            return null;
        } 
    }
}