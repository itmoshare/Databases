using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using RestApi.Models;

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
        [HttpPut("{unitStopId}/{routeNumber}")]
        public IActionResult CreateTimeTableItem(int unitStopId, string routeNumber)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete relation between bus stop and route number
        /// </summary>
        [HttpDelete]
        public IActionResult DeleteTimeTableItem([FromBody]TimeTableItem timeTableItem)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get relations by unit stop and route id.
        /// </summary>
        [HttpGet("{unitStopId>/{routeNumber}")]
        public IEnumerable<TimeTableItem> GetRoutes(int unitStopId, string routeNumber)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public IActionResult UpdateTimeTableItem([FromBody]TimeTableItem lastItem, [FromBody]TimeTableItem newItem)
        {
            throw new NotImplementedException();
        }
    }
}