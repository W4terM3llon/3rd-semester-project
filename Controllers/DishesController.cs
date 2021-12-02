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
    public class DishesController : ControllerBase
    {
        private readonly IDishRepository _dishRepository;
        private readonly PermissionValidation _permissionValidation;

        public DishesController(RestaurantSystemContext context, UserManager<User> userManager)
        {
            _dishRepository = new DishRepository(context);
            _permissionValidation = new PermissionValidation(context, userManager);
        }

        // GET: api/Dishes
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Dish>>> GetDish([FromQuery] DishRequest dishQuery)
        {
            var dishes = await _dishRepository.GetAllAsync(dishQuery);
            return Ok(dishes);
        }

        // GET: api/Dishes/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Dish>> GetDish(string id)
        {
            var dish = await _dishRepository.GetAsync(id);

            if (dish == null)
            {
                return NotFound();
            }

            return Ok(dish);
        }

        // PUT: api/Dishes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<IActionResult> PutDish(string id, DishRequest dishRequest)
        {
            var oldDish = await _dishRepository.GetAsync(id);
            if (id != dishRequest.Id || oldDish.Restaurant.Id==dishRequest.Restaurant)
            {
                return BadRequest();
            }

            var booking = await _dishRepository.ConvertAlterDishRequest(dishRequest);
            if (booking == null)
            {
                return BadRequest();
            }

            if (await _dishRepository.IfExist(id))
            {
                var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
                if (!await _permissionValidation.isManagerDishOwnerAsync(id, currentUserEmail))
                {
                    return Unauthorized();
                }
            }
            else
            {
                return NotFound();
            }

            var dish = await _dishRepository.UpdateAsync(booking);

            if (dish == null)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/Dishes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<ActionResult<DishRequest>> PostDish(DishRequest dishRequest)
        {
            var booking = await _dishRepository.ConvertAlterDishRequest(dishRequest);
            if (booking == null)
            {
                return NotFound();
            }

            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            if (!await _permissionValidation.isManagerRestaurantOwnerAsync(dishRequest.Restaurant, currentUserEmail))
            {
                return Unauthorized();
            }

            var dish = await _dishRepository.CreateAsync(booking);

            if (dish == null)
            {
                return BadRequest();
            }

            return CreatedAtAction("GetDish", new { id = dishRequest.Id }, dishRequest);
        }

        // DELETE: api/Dishes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<IActionResult> DeleteDish(string id)
        {
            if (await _dishRepository.IfExist(id))
            {
                var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
                if (!await _permissionValidation.isManagerDishOwnerAsync(id, currentUserEmail))
                {
                    return Unauthorized();
                }
            }
            else
            {
                return NotFound();
            }

            var dish = await _dishRepository.DeleteAsync(id);

            if (dish == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
