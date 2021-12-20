﻿using System;
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
        private readonly IPermissionValidation _permissionValidation;

        public DishesController(IDishRepository dishRepository, IPermissionValidation permissionValidation)
        {
            _dishRepository = dishRepository;
            _permissionValidation = permissionValidation;
        }

        // GET: api/Dishes
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DishResponseDTO>>> GetDish([FromQuery] string restaurantId)
        {
            if (restaurantId == null)
            {
                return BadRequest(new { Error = "Restaurant id required" });
            }

            var dishes = await _dishRepository.GetAllAsync(restaurantId);
            return Ok(dishes.Select(b => (DishResponseDTO)b).ToList());
        }

        // PUT: api/Dishes/5
        [HttpPut("{id}")]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<ActionResult<DishResponseDTO>> PutDish(string id, DishRequestDTO dishRequest)
        {
            if (!await _dishRepository.IfExist(id))
            {
                return NotFound(new { Error = "Dish with given id not found" });
            }

            var oldDish = await _dishRepository.GetAsync(id);
            if (oldDish.Restaurant.Id != dishRequest.Restaurant)
            {
                return BadRequest(new { Error = "Restaurant can not be changed" });
            }

            var currentUserSystemId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Actor).Value;
            if (!await _permissionValidation.isManagerDishOwnerAsync(id, currentUserSystemId))
            {
                return Unauthorized(new { Error = "This dish does not belong to your restaurant" });
            }
            
            var dish = await _dishRepository.ConvertAlterDishRequest(dishRequest, id);
            if (dish == null)
            {
                return NotFound(new { Error = "One of dish dependencies not found" });
            }

            var updatedDish = await _dishRepository.UpdateAsync(dish);

            return Ok((DishResponseDTO)updatedDish);
        }

        // PUT: api/Dishes/5
        [HttpPatch("{id}")]
        [Authorize(Roles = "RestaurantManager, Customer, RestaurantEveryDayUse")]
        public async Task<ActionResult<DishResponseDTO>> PatchDish(string id)
        {
            if (!await _dishRepository.IfExist(id))
            {
                return NotFound(new { Error = "Dish with given id not found" });
            }

            var updatedDish = await _dishRepository.IncrementLikeAsync(id);

            return Ok((DishResponseDTO)updatedDish);
        }

        // POST: api/Dishes
        [HttpPost]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<ActionResult<DishResponseDTO>> PostDish(DishRequestDTO dishRequest)
        {
            var booking = await _dishRepository.ConvertAlterDishRequest(dishRequest, null);
            if (booking == null)
            {
                return NotFound(new { Error = "One of dish dependencies not found" });
            }

            var currentUserSystemId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Actor).Value;
            if (!await _permissionValidation.isManagerRestaurantOwnerAsync(dishRequest.Restaurant, currentUserSystemId))
            {
                return Unauthorized(new { Error = "Given restaurant is not yours" });
            }

            var dish = await _dishRepository.CreateAsync(booking);

            return CreatedAtAction("GetDish", new { id = dish.Id }, (DishResponseDTO)dish);
        }

        // DELETE: api/Dishes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<IActionResult> DeleteDish(string id)
        {
            if (!await _dishRepository.IfExist(id))
            {
                return NotFound(new { Error = "Dish with given id not found" });
            }

            var currentUserSystemId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Actor).Value;
            if (!await _permissionValidation.isManagerDishOwnerAsync(id, currentUserSystemId))
            {
                return Unauthorized(new { Error = "This dish does not belong to your restaurant" });
            }
            
            var dish = await _dishRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}