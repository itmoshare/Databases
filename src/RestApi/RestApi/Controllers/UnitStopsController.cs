using System;
using System.Collections.Generic;
using System.Linq;
using Cassandra;
using Microsoft.AspNetCore.Mvc;
using RestApi.Models;

namespace RestApi.Controllers
{
    [Produces("application/json")]
    [Route("api/UnitStops")]
    public class UnitStopsController : Controller
    {
        private readonly ICluster _cluster;

        public const string KeySpace = "db";

        public UnitStopsController(ICluster cluster)
        {
            _cluster = cluster;
        }

        // GET: api/UnitStops/5
        [HttpGet("{id}")]
        public UnitStop Get(int id)
        {
            using (var session = _cluster.Connect(KeySpace))
            {
                Row res = session.Execute($"select * from bus_stops where id = {id};").FirstOrDefault();
                if (res == null)
                    return null;
                return new UnitStop
                {
                    Id = id,
                    Title = res.GetValue<string>("title"),
                    Latitude = res.GetValue<double>("latitude"),
                    Longitude = res.GetValue<double>("longitude")
                };
            }
        }

        // POST: api/UnitStops
        [HttpPost]
        public IActionResult Post([FromBody]UnitStop value)
        {
            using (var session = _cluster.Connect(KeySpace))
            {
                session.Execute($"insert into bus_stops values({value.Title}, {value.Latitude}, {value.Longitude});");
                return Ok();
            }
        }

        // PUT: api/UnitStops/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UnitStop value)
        {
            if (value.Id != id)
                return BadRequest();
            using (var session = _cluster.Connect(KeySpace))
            {
                session.Execute($"update bus_stops " +
                                $"set title={value.Title}, latitude={value.Latitude}, longitude={value.Longitude} " +
                                $"where id={id};");
                return Ok();
            }
        }

        // DELETE: api/UnitStops/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (var session = _cluster.Connect(KeySpace))
            {
                session.Execute($"delete from bus_stops where id={id};");
                return Ok();
            }
        }

        /// <summary>
        /// Get stops near position (stored procedure!)
        /// </summary>
        [HttpGet("near/{x}/{y}/{radius}")]
        public IEnumerable<UnitStop> GetNear(double x, double y, double radius)
        {
            using (var session = _cluster.Connect(KeySpace))
            {
                throw new NotImplementedException();
            }
        }
    }
}
