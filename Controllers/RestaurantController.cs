using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models;
using RestaurantSystem.Models.Repositories;
using RestaurantSystem.Models.Requests;
using RestaurantSystem.Services;

namespace RestaurantSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantRepository _restaurantRepository;
        private readonly PermissionValidation _permissionValidation;

        public RestaurantController(RestaurantSystemContext context, UserManager<User> userManager)
        {
            _restaurantRepository = new RestaurantRepository(context, userManager);
            _permissionValidation = new PermissionValidation(context, userManager);
        }

        // GET: api/Restaurants
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetRestaurantsAsync()
        {
            var restaurants = await _restaurantRepository.GetAllAsync();
            return Ok(restaurants);
        }

        // GET: api/Restaurants/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Restaurant>> GetRestaurantAsync(string id)
        {
            if (!await _restaurantRepository.IfExist(id))
            {
                return NotFound(new { Error = "Restaurant with given id not found" });
            }

            var restaurant = await _restaurantRepository.GetAsync(id);
            return Ok(restaurant);
        }

        // PUT: api/Restaurants/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<ActionResult> PutRestaurantAsync(string id, RestaurantRequest request)
        {
            if (await _restaurantRepository.IfExist(id))
            {
                return NotFound(new { Error = "Restaurant with given id not found" });
            }

            if (id != request.Id)
            {
                return BadRequest(new { Error = "Id can not be changed" });
            }

            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            if (!await _permissionValidation.isManagerRestaurantOwnerAsync(id, currentUserEmail))
            {
                return Unauthorized(new { Error = "This restaurant is not managed by your account" });
            }
            
            var restaurant = await _restaurantRepository.ConvertAlterRestaurantRequest(request, currentUserEmail);
            var returnedRestaurant = await _restaurantRepository.UpdateAsync(restaurant);

            return Ok(returnedRestaurant);
        }

        // POST: api/Restaurants
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<ActionResult<Restaurant>> PostRestaurantAsync(RestaurantRequest request)
        {
            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            var restaurant = await _restaurantRepository.ConvertAlterRestaurantRequest(request, currentUserEmail);
            var created = await _restaurantRepository.CreateAsync(restaurant, request.EveryDayUseAccountEmail);

            if (created == null)
            {
                return BadRequest(new { Error = "Could not create restaurant, email for every day use account may already exist or is incorrect" });
            }

            return CreatedAtAction("GetRestaurant", new { id = created.Id }, created);
        }

        // DELETE: api/Restaurants/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<IActionResult> DeleteRestaurantAsync(string id)
        {
            if (await _restaurantRepository.IfExist(id))
            {
                var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
                if (!await _permissionValidation.isManagerRestaurantOwnerAsync(id, currentUserEmail))
                {
                    return Unauthorized(new { Error = "This restaurant is not managed by your account" });
                }
            }
            else
            {
                return NotFound(new { Error = "Restaurant with given id not found" });
            }

            var returnedRestaurant = await _restaurantRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
