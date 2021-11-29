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
    public class TablesController : ControllerBase
    {
        private readonly TableRepository _tableRepository;
        private readonly PermissionValidation _permissionValidation;

        public TablesController(RestaurantSystemContext context, UserManager<User> userManager)
        {
            _tableRepository = new TableRepository(context);
            _permissionValidation = new PermissionValidation(context, userManager);
        }

        // GET: api/Tables
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Table>>> GetTableAsync([FromQuery] TableRequest tableQuery)
        {
            var restaurants = await _tableRepository.GetAllAsync(tableQuery);
            return Ok(restaurants);
        }

        // GET: api/Tables/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Table>> GetTableAsync(string id)
        {
            var restaurant = await _tableRepository.GetAsync(id);

            if (restaurant == null)
            {
                return NotFound();
            }

            return Ok(restaurant);
        }

        // PUT: api/Tables/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<IActionResult> PutTableAsync(string id, TableRequest tableRequest)
        {
            var oldTable = await _tableRepository.GetAsync(id);
            if (id != tableRequest.Id || tableRequest.Restaurant != oldTable.Restaurant.Id)
            {
                return BadRequest();
            }

            var table = await _tableRepository.ConvertAlterTableRequest(tableRequest);
            if (table == null)
            {
                return BadRequest();
            }

            if (await _tableRepository.IfExist(id))
            {
                var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
                if (!await _permissionValidation.isManagerTableOwnerAsync(id, currentUserEmail))
                {
                    return Unauthorized();
                }
            }
            else
            {
                return NotFound();
            }

            var updated = await _tableRepository.UpdateAsync(table);

            if (updated == null)
            {
                return BadRequest();
            }

            return NoContent();
        }

        
        // POST: api/Tables
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        [HttpPost]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<ActionResult<Table>> PostTableAsync(TableRequest tableRequest)
        {
            var table = await _tableRepository.ConvertAlterTableRequest(tableRequest);
            if (table == null)
            {
                return NotFound();
            }

            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            if (!await _permissionValidation.isManagerRestaurantOwnerAsync(tableRequest.Restaurant, currentUserEmail))
            {
                return Unauthorized();
            }

            var created = await _tableRepository.CreateAsync(table);

            if (created == null) 
            {
                return BadRequest();
            }

            return CreatedAtAction("GetTable", new { id = created.Id }, created);
        }
        

        // DELETE: api/Tables/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<IActionResult> DeleteTableAsync(string id)
        {
            if (await _tableRepository.IfExist(id))
            {
                var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
                if (!await _permissionValidation.isManagerTableOwnerAsync(id, currentUserEmail))
                {
                    return Unauthorized();
                }
            }
            else
            {
                return NotFound();
            }

            var deleted = await _tableRepository.DeleteAsync(id);

            if (deleted == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
