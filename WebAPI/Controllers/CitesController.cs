using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitesController : ControllerBase
    {
        // GET: api/<CitesController>
        [HttpGet]
        public IEnumerable<string> Get()    
        {
            return new string[] { "Mumbai", "Bangalore" };
        }

        // GET api/<CitesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "Bangalore";
        }

        // POST api/<CitesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT api/<CitesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CitesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
