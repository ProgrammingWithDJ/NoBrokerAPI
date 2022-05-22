using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitesController : ControllerBase
    {
        private readonly DataContext dc;
        public CitesController(DataContext dc)
        {
            this.dc = dc;
        }   
        // GET: api/<CitesController>
        [HttpGet]
        public async Task<IActionResult> GetCities()    
        {
            var cities = await dc.Cities.ToListAsync();
            return Ok(cities);
        }

        [HttpPost("add/{cityname}")]
        public async Task<IActionResult> AddCity(string cityName)
        {
            City city = new City();
            city.Name = cityName;

            await dc.Cities.AddAsync(city);

           await dc.SaveChangesAsync();

            return Ok(city);
        }

        [HttpPost("Post")]
        public async Task<IActionResult> AddCityForm(City city)
        {
            await dc.Cities.AddAsync(city);

            await dc.SaveChangesAsync();

            return Ok(city);
        }

        [HttpDelete("delete/{Id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            var city = await dc.Cities.FindAsync(id);
            

              dc.Cities.Remove(city);
             await dc.SaveChangesAsync();

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
