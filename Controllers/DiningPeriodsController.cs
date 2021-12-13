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
    public class DiningPeriodsController : ControllerBase
    {
        private readonly IDiningPeriodRepository _diningPeriodRepository;
        private readonly IPermissionValidation _permissionValidation;

        public DiningPeriodsController(IDiningPeriodRepository diningPeriodRepository, IPermissionValidation permissionValidation)
        {
            _diningPeriodRepository = diningPeriodRepository;
            _permissionValidation = permissionValidation;
        }

        // GET: api/DiningPeriods
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DiningPeriodResponseDTO>>> GetDiningPeriod([FromQuery] string restaurantId)
        {
            if (restaurantId == null)
            {
                return BadRequest(new { Error = "Restaurant id required" });
            }

            var diningPeriods= await _diningPeriodRepository.GetAllAsync(restaurantId);
            return Ok(diningPeriods.Select(b => (DiningPeriodResponseDTO)b).ToList());
        }

        // PUT: api/DiningPeriods/5
        [HttpPut("{id}")]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<ActionResult<DiningPeriodResponseDTO>> PutDiningPeriod(string id, DiningPeriodRequestDTO diningPeriodRequest)
        {
            if (await _diningPeriodRepository.IfExist(id))
            {
                return NotFound(new { Error = "Dining period with given id not found" });
            }

            var oldDiningPeriod = await _diningPeriodRepository.GetAsync(id);
            if (diningPeriodRequest.Restaurant != oldDiningPeriod.Restaurant.Id)
            {
                return BadRequest(new { Error = "Restaurant can not be changed" });
            }

            var currentUserSystemId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Actor).Value;
            if (!await _permissionValidation.isManagerDiningPeriodOwnerAsync(id, currentUserSystemId))
            {
                return Unauthorized(new { Error = "This dining period does not belong to your restaurant" });
            }

            var diningPeriod = await _diningPeriodRepository.ConvertAlterDiningPeriodRequest(diningPeriodRequest, id);
            if (diningPeriod == null)
            {
                return NotFound(new { Error = "One of dining period dependencies not found" });
            }

            if (!_diningPeriodRepository.IfDiningTimeCorrent(diningPeriod))
            {
                return BadRequest(new { Error = "Incorrect time data" });
            }

            if (await _diningPeriodRepository.IfPeriodsOverlap(diningPeriod))
            {
                return Conflict(new { Error = "Dining period collides with other already existing dining periods" });
            }

            var updatedDiningPeriod = await _diningPeriodRepository.UpdateAsync(diningPeriod);

            return Ok((DiningPeriodResponseDTO)updatedDiningPeriod);
        }

        // POST: api/DiningPeriods
        [HttpPost]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<ActionResult<DiningPeriodResponseDTO>> PostDiningPeriod(DiningPeriodRequestDTO request)
        {
            var diningPeriod = await _diningPeriodRepository.ConvertAlterDiningPeriodRequest(request, null);
            if (diningPeriod == null)
            {
                return NotFound(new { Error = "One of dining period dependencies not found" });
            }

            var currentUserSystemId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Actor).Value;
            if (!await _permissionValidation.isManagerRestaurantOwnerAsync(request.Restaurant, currentUserSystemId))
            {
                return Unauthorized(new { Error = "Restaurant with given Id is not yours" });
            }

            if (!_diningPeriodRepository.IfDiningTimeCorrent(diningPeriod))
            {
                return BadRequest(new { Error = "Incorrect time data" });
            }

            if (await _diningPeriodRepository.IfPeriodsOverlap(diningPeriod))
            {
                return Conflict(new { Error = "Dining period collides with other already existing dining periods" });
            }

            var createdDiningPeriod = await _diningPeriodRepository.CreateAsync(diningPeriod);

            return CreatedAtAction("GetDiningPeriod", new { id = diningPeriod.Id }, (DiningPeriodResponseDTO)diningPeriod);
        }

        // DELETE: api/DiningPeriods/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<IActionResult> DeleteDiningPeriod(string id)
        {
            if (await _diningPeriodRepository.IfExist(id))
            {
                var currentUserSystemId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Actor).Value;
                if (!await _permissionValidation.isManagerDiningPeriodOwnerAsync(id, currentUserSystemId))
                {
                    return Unauthorized(new { Error = "This dining period does not belong to your restaurant" });
                }
            }
            else
            {
                return NotFound(new { Error = "Dining period with given id not found" });
            }

            var returnedRestaurant = await _diningPeriodRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}