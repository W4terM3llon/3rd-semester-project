using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models;
using RestaurantSystem.Models.Repositories;
using RestaurantSystem.Models.Requests;

namespace RestaurantSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantRepository _restaurantRepository;

        public RestaurantController(RestaurantSystemContext context)
        {
            _restaurantRepository = new RestaurantRepository(context);
        }

        // GET: api/Restaurants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetRestaurantsAsync()
        {
            var restaurants = await _restaurantRepository.GetAllAsync();
            return Ok(restaurants);
        }

        // GET: api/Restaurants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurant>> GetRestaurantAsync(string id)
        {
            var restaurant = await _restaurantRepository.GetAsync(id);

            if (restaurant == null)
            {
                return NotFound();
            }

            return Ok(restaurant);
        }

        // PUT: api/Restaurants/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult> PutRestaurantAsync(string id, Restaurant restaurant)
        {
            if (id != restaurant.Id)
            {
                return BadRequest();
            }

            var returnedRestaurant = await _restaurantRepository.UpdateAsync(restaurant);

            if (returnedRestaurant == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Restaurants
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Restaurant>> PostRestaurantAsync(Restaurant restaurant)
        {

            restaurant.Id = new Random().Next(1, 1000).ToString(); //Replace by real id generator
            restaurant.Address.Id = new Random().Next(1, 1000).ToString(); //Replace by real id generator

            await _restaurantRepository.CreateAsync(restaurant);

            return CreatedAtAction("GetRestaurant", new { id = restaurant.Id }, restaurant);
        }

        // DELETE: api/Restaurants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurantAsync(string id)
        {
            var returnedRestaurant = await _restaurantRepository.DeleteAsync(id);

            if (returnedRestaurant == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
