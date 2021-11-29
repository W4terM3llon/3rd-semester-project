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
            //var queryParams = HttpContext.Request.Query.ToDictionary(query => query.Key.ToString(), query => query.Value.ToString());
            var restaurants = await _restaurantRepository.GetAllAsync();
            return Ok(restaurants);
        }

        // GET: api/Restaurants/5
        [HttpGet("{id}")]
        [AllowAnonymous]
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
        [Authorize(Roles = "RestaurantManager")]
        public async Task<ActionResult> PutRestaurantAsync(string id, RestaurantRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }


            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            if (await _restaurantRepository.IfExist(id))
            {
                if (!await _permissionValidation.isManagerRestaurantOwnerAsync(id, currentUserEmail))
                {
                    return Unauthorized();
                }
            }
            else
            { 
                return NotFound(); 
            }

            var restaurant = await _restaurantRepository.ConvertAlterRestaurantRequest(request, currentUserEmail);
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
        [Authorize(Roles = "RestaurantManager")]
        public async Task<ActionResult<Restaurant>> PostRestaurantAsync(RestaurantRequest request)
        {
            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            var restaurant = await _restaurantRepository.ConvertAlterRestaurantRequest(request, currentUserEmail);
            var created = await _restaurantRepository.CreateAsync(restaurant, request.EveryDayUseAccountEmail);

            if (created != null)
            {
                return CreatedAtAction("GetRestaurant", new { id = created.Id }, created);
            }
            else 
            {
                return BadRequest();
            }
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
                    return Unauthorized();
                }
            }
            else
            {
                return NotFound();
            }

            var returnedRestaurant = await _restaurantRepository.DeleteAsync(id);

            if (returnedRestaurant == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
