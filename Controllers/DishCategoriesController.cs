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
    public class DishCategoriesController : ControllerBase
    {
        private readonly IDishCategoryRepository _dishCategoryRepository;
        private readonly PermissionValidation _permissionValidation;

        public DishCategoriesController(RestaurantSystemContext context, UserManager<User> userManager)
        {
            _dishCategoryRepository = new DishCategoryRepository(context);
            _permissionValidation = new PermissionValidation(context, userManager);
        }


        // GET: api/DishCategories
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DishCategory>>> GetDishCategory()
        {
            var dishCategories = await _dishCategoryRepository.GetAllAsync();
            return Ok(dishCategories);
        }
        // GET: api/DishCategories/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<DishCategory>> GetDishCategory(string id)
        {
            var dishCategory = await _dishCategoryRepository.GetAsync(id);

            if (dishCategory == null)
            {
                return NotFound(new { Error = "Dish with given id not found" });
            }

            return Ok(dishCategory);
        }
        /*

        // PUT: api/DishCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<IActionResult> PutDishCategory(string id, DishCategoryRequest request)
        {
            var oldDishCategory = await _dishCategoryRepository.GetAsync(id);
            if (id != request.Id || request.Restaurant != oldDishCategory.Restaurant.Id)
            {
                return BadRequest();
            }

            var booking = await _dishCategoryRepository.ConvertAlterDishRequest(request);
            if (booking == null)
            {
                return BadRequest();
            }

            if (await _dishCategoryRepository.IfExist(request.Id))
            {
                var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
                if (!await _permissionValidation.isManagerDishCategoryOwnerAsync(id, currentUserEmail))
                {
                    return Unauthorized();
                }
            }
            else
            {
                return NotFound();
            }

            var dish = await _dishCategoryRepository.UpdateAsync(booking);

            if (dish == null)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/DishCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<ActionResult<DishCategory>> PostDishCategory(DishCategoryRequest request)
        {
            var booking = await _dishCategoryRepository.ConvertAlterDishRequest(request);
            if (booking == null)
            {
                return BadRequest();
            }

            if (await _dishCategoryRepository.IfExist(request.Id))
            {
                var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
                if (!await _permissionValidation.isManagerRestaurantOwnerAsync(request.Restaurant, currentUserEmail))
                {
                    return Unauthorized();
                }
            }
            else
            {
                return NotFound();
            }

            var dish = await _dishCategoryRepository.CreateAsync(booking);

            if (dish == null)
            {
                return BadRequest();
            }

            return CreatedAtAction("GetDishCategory", new { id = booking.DbId }, booking);
        }

        // DELETE: api/DishCategories/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<IActionResult> DeleteDishCategory(string id)
        {
            if (await _dishCategoryRepository.IfExist(id))
            {
                var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
                if (!await _permissionValidation.isManagerDishCategoryOwnerAsync(id, currentUserEmail))
                {
                    return Unauthorized();
                }
            }
            else
            {
                return NotFound();
            }

            var dish = await _dishCategoryRepository.DeleteAsync(id);
            
            if (dish == null)
            {
                return NotFound();
            }

            return NoContent();
        }*/
    }
}
