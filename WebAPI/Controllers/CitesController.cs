using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Data.Repo;
using WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitesController : ControllerBase
    {
        private readonly DataContext dc;
        private readonly ICityRepository repo;
        public CitesController(ICityRepository repo)
        {
          
            this.repo = repo;
        }   
        // GET: api/<CitesController>
        [HttpGet]
        public async Task<IActionResult> GetCities()    
        {
            var cities = await repo.GetCitiesAsync();
            return Ok(cities);
        }

        

        [HttpPost("Post")]
        public async Task<IActionResult> AddCityForm(City city)
        {
            repo.AddCity(city);

            await repo.SaveAsync();

            return StatusCode(201);
        }

        [HttpDelete("delete/{Id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            //  var city = await dc.Cities.FindAsync(id);

            repo.DeleteCity(id);
            

              
             await repo.SaveAsync();

            return Ok(id);

            
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
