using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Data.Repo;
using WebAPI.Dtos;
using WebAPI.Interfaces;
using WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitesController : ControllerBase
    {
        private readonly DataContext dc;
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        public CitesController(IUnitOfWork uow, IMapper mapper)
        {
            this.mapper = mapper;
            this.uow = uow;
        }   
        // GET: api/<CitesController>
        [HttpGet]
        public async Task<IActionResult> GetCities()    
        {
            var cities = await uow.CityRepository.GetCitiesAsync();

            var citiesDto = mapper.Map<IEnumerable<CityDto>>(cities);

            //var citiesDto = from c in cities
            //                select new CityDto()
            //                {
            //                    Id = c.Id,
            //                    Name = c.Name
            //                };

            return Ok(citiesDto);
        }

        

        [HttpPost("Post")]
        public async Task<IActionResult> AddCityForm(CityDto cityDto)
        {
            var city = mapper.Map<City>(cityDto);
            city.LastUpdatedBy = 1;
            city.LastUpdatedOn = DateTime.Now;

            //var city = new City
            //{
            //    Name = cityDto.Name,
            //    LastUpdatedBy = 1,
            //    LastUpdatedOn = DateTime.Now
            //};

            uow.CityRepository.AddCity(city);

            await uow.SaveAsync();

            return StatusCode(201);
        }

        [HttpDelete("delete/{Id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            //  var city = await dc.Cities.FindAsync(id);

            uow.CityRepository.DeleteCity(id);
            

              
             await uow.SaveAsync();

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
