using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestApi.Models;

namespace RestApi.Controllers
{
    [Produces("application/json")]
    [Route("api/BusStops")]
    public class UnitStopsController : Controller
    {
        // GET: api/BusStops/5
        [HttpGet("{id}")]
        public UnitStop Get(int id)
        {
            throw new NotImplementedException();
        }

        // POST: api/BusStops
        [HttpPost]
        public void Post([FromBody]UnitStop value)
        {
            throw new NotImplementedException();
        }

        // PUT: api/BusStops/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]UnitStop value)
        {
            throw new NotImplementedException();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get stops near position (stored procedure!)
        /// </summary>
        [HttpGet("near/{x}/{y}/{radius}")]
        public IEnumerable<UnitStop> GetNear(double x, double y, double radius)
        {
            throw new NotImplementedException();
        }
    }
}
