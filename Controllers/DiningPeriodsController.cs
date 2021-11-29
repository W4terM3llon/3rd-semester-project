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
        private readonly DiningPeriodRepository _diningPeriodRepository;
        private readonly PermissionValidation _permissionValidation;

        public DiningPeriodsController(RestaurantSystemContext context, UserManager<User> userManager)
        {
            _diningPeriodRepository = new DiningPeriodRepository(context);
            _permissionValidation = new PermissionValidation(context, userManager);
        }

        // GET: api/DiningPeriods
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DiningPeriod>>> GetDiningPeriod()
        {
            var diningPeriods= await _diningPeriodRepository.GetAllAsync();
            return Ok(diningPeriods);
        }

        // GET: api/DiningPeriods/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<DiningPeriod>> GetDiningPeriod(string id)
        {

            var diningPeriod = await _diningPeriodRepository.GetAsync(id);

            if (diningPeriod == null)
            {
                return NotFound();
            }

            return Ok(diningPeriod);
        }

        // PUT: api/DiningPeriods/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<IActionResult> PutDiningPeriod(string id, DiningPeriodRequest diningPeriodRequest)
        {
            var oldDiningPeriod = await _diningPeriodRepository.GetAsync(id);
            if (id != diningPeriodRequest.Id || diningPeriodRequest.Restaurant != oldDiningPeriod.Restaurant.Id)
            {
                return BadRequest();
            }

            if (await _diningPeriodRepository.IfExist(id))
            {
                var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
                if (!await _permissionValidation.isManagerDiningPeriodOwnerAsync(id, currentUserEmail))
                {
                    return Unauthorized();
                }
            }
            else 
            {
                return NotFound();
            }

            var diningPeriod = await _diningPeriodRepository.ConvertAlterDiningPeriodRequest(diningPeriodRequest);
            if (diningPeriod == null)
            {
                return BadRequest();
            }

            var updatedDiningPeriod = await _diningPeriodRepository.UpdateAsync(diningPeriod);

            if (updatedDiningPeriod == null)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/DiningPeriods
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<ActionResult<DiningPeriod>> PostDiningPeriod(DiningPeriodRequest request)
        {
            var diningPeriod = await _diningPeriodRepository.ConvertAlterDiningPeriodRequest(request);
            if (diningPeriod == null)
            {
                return BadRequest();
            }

            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            if (!await _permissionValidation.isManagerRestaurantOwnerAsync(request.Restaurant, currentUserEmail))
            {
                return Unauthorized();
            }

            var createdDiningPeriod = await _diningPeriodRepository.CreateAsync(diningPeriod);
            if (createdDiningPeriod == null)
            {
                return BadRequest();
            }

            return CreatedAtAction("GetDiningPeriod", new { id = diningPeriod.Id }, diningPeriod);
        }

        // DELETE: api/DiningPeriods/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<IActionResult> DeleteDiningPeriod(string id)
        {
            if (await _diningPeriodRepository.IfExist(id))
            {
                var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
                if (!await _permissionValidation.isManagerDiningPeriodOwnerAsync(id, currentUserEmail))
                {
                    return Unauthorized();
                }
            }
            else
            {
                return NotFound();
            }

            var returnedRestaurant = await _diningPeriodRepository.DeleteAsync(id);

            if (returnedRestaurant == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}