using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestApi.Models;
using ServiceStack.Redis;

namespace RestApi.Controllers
{
    [Produces("application/json")]
    [Route("api/unit")]
    public class UnitPositionController : Controller
    {
        private readonly IRedisClientsManager _redisManager;

        public UnitPositionController(IRedisClientsManager redisManager)
        {
            _redisManager = redisManager;
        }

        // GET api/unit/5
        [HttpGet("{id}")]
        public Position Get(int id)
        {
            using (var redis = _redisManager.GetClient())
            {
                return redis.ContainsKey(id.ToString()) ? 
                    Position.Parse(redis.Get<string>(id.ToString())) : 
                    null; 
            }
        }

        // PUT api/unit/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Position value)
        {
            using (var redis = _redisManager.GetClient())
            {
                redis.Set(id.ToString(), value);
            }
        }

        /// <summary>
        /// Get unit ids near some point (in radius) (stored procedure, if redis can store procedures!)
        /// </summary>
        [HttpGet("near/{x}/{y}/{radius}")]
        public IEnumerable<int> GetNearUnits(double x, double y, double radius)
        {
            using(var redis = _redisManager.GetClient())
            {
                var redisResponse = redis.ExecLuaSha("2a003c9ed98db5ab1401a9372ec19ec037a719e7", x.ToString(), 
                    y.ToString(), radius.ToString());
                return redisResponse.Children.Select(c => Int32.Parse(c.Text));
            }
        }
    }
}