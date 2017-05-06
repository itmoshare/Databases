using System;
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
            throw new NotImplementedException();
        }
    }
}