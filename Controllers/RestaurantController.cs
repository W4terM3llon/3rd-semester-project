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
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IPermissionValidation _permissionValidation;

        public RestaurantController(IRestaurantRepository restaurantRepository, IPermissionValidation permissionValidation)
        {
            _restaurantRepository = restaurantRepository;
            _permissionValidation = permissionValidation;
        }

        // GET: api/Restaurants
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<RestaurantResponseDTO>>> GetRestaurantsAsync()
        {
            var restaurants = await _restaurantRepository.GetAllAsync();
            return Ok(restaurants.Select(b => (RestaurantResponseDTO)b).ToList());
        }

        // PUT: api/Restaurants/5
        [HttpPut("{id}")]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<ActionResult<RestaurantResponseDTO>> PutRestaurantAsync(string id, RestaurantRequestDTO request)
        {
            if (!await _restaurantRepository.IfExist(id))
            {
                return NotFound(new { Error = "Restaurant with given id not found" });
            }

            var currentUserSystemId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Actor).Value;
            if (!await _permissionValidation.isManagerRestaurantOwnerAsync(id, currentUserSystemId))
            {
                return Unauthorized(new { Error = "This restaurant is not managed by your account" });
            }
            
            var restaurant = await _restaurantRepository.ConvertAlterRestaurantRequest(request, currentUserSystemId, id);
            var returnedRestaurant = await _restaurantRepository.UpdateAsync(restaurant);

            return Ok((RestaurantResponseDTO)returnedRestaurant);
        }

        // POST: api/Restaurants
        [HttpPost]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<ActionResult<RestaurantResponseDTO>> PostRestaurantAsync(RestaurantRequestDTO request)
        {
            var currentUserSystemId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Actor).Value;
            var restaurant = await _restaurantRepository.ConvertAlterRestaurantRequest(request, currentUserSystemId, null);
            var created = await _restaurantRepository.CreateAsync(restaurant, request.EveryDayUseAccountEmail, request.EveryDayUseAccountPassword);

            if (created == null)
            {
                return BadRequest(new { Error = "Could not create restaurant, email and password for every day use account may already exist are incorrect" });
            }

            return CreatedAtAction("GetRestaurant", new { id = created.Id }, (RestaurantResponseDTO)created);
        }

        // DELETE: api/Restaurants/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<IActionResult> DeleteRestaurantAsync(string id)
        {
            if (!await _restaurantRepository.IfExist(id))
            {
                return NotFound(new { Error = "Restaurant with given id not found" });
            }

            var currentUserSystemId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Actor).Value;
            if (!await _permissionValidation.isManagerRestaurantOwnerAsync(id, currentUserSystemId))
            {
                return Unauthorized(new { Error = "This restaurant is not managed by your account" });
            }
            

            var returnedRestaurant = await _restaurantRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
