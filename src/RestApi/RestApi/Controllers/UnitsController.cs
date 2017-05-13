using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RestApi.Models;
using ServiceStack;

namespace RestApi.Controllers
{
    [Produces("application/json")]
    [Microsoft.AspNetCore.Mvc.Route("api/units")]
    public class UnitsController : Controller
    {
        public const string DbName = "test";
        public const string UnitsCollectionName = "units";

        private readonly IMongoClient _mongoClient;

        public UnitsController(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        private IMongoCollection<Unit> GetUnitsCollection()
        {
            return _mongoClient.GetDatabase(DbName).GetCollection<Unit>(UnitsCollectionName);
        }

        // GET: api/Units
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(GetUnitsCollection().Find(x => true).ToList());
        }

        // GET: api/Units/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(GetUnitsCollection().Find(x => x.Id == id).FirstOrDefault());
        }
        
        // POST: api/Units
        [HttpPost]
        public IActionResult Post([FromBody]Unit value)
        {
            GetUnitsCollection().InsertOne(value);
            return Ok();
        }
        
        // PUT: api/Units/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Unit value)
        {
            if (id != value.Id)
                return BadRequest();
            try
            {
                GetUnitsCollection().ReplaceOne(x => x.Id == id, value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
            return Ok();
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                GetUnitsCollection().DeleteOne(x => x.Id == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
            return Ok();
        }

        /// <summary>
        /// Get units by type (stored procedure!)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("type/{type}")]
        public IActionResult GetByType(string type)
        {
            var db = _mongoClient.GetDatabase(DbName);
            var function = $"getUnitsByType(\'{type}\')";
            var result = db.RunCommand<List<Unit>>(new JsonCommand<List<Unit>>($"{{ eval:\"{function}\"}}"));
            return Ok(result);
        }
    }
}
