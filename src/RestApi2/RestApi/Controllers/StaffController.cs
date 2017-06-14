using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestApi.Middleware;
using RestApi.Model;

namespace RestApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Staff")]
    public class StaffController : Controller
    {

        private IStaffDriver _staffDriver;

        public StaffController(IStaffDriver staffDriver)
        {
            _staffDriver = staffDriver;
        } 
        // GET: api/Staff
        [HttpGet]
        public IEnumerable<Staff> Get()
        {
            return _staffDriver.ListAll();
        }

        // GET: api/Staff/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var result = _staffDriver.Get(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }
        
        // POST: api/Staff
        [HttpPost]
        public IActionResult Post([FromBody]Staff value)
        {
            _staffDriver.Add(value);
            return Ok();
        }

        // PUT: api/Staff/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Staff value)
        {
            _staffDriver.Update(id, value);
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _staffDriver.Delete(id);
            return Ok();
        }
    }
}
