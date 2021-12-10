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
        private readonly ITableRepository _tableRepository;
        private readonly IPermissionValidation _permissionValidation;

        public TablesController(ITableRepository tableRepository, IPermissionValidation permissionValidation)
        {
            _tableRepository = tableRepository;
            _permissionValidation = permissionValidation;
        }

        // GET: api/Tables
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TableResponseDTO>>> GetTableAsync([FromQuery] string restaurantId, string id)
        {
            if (restaurantId == null)
            {
                return BadRequest(new { Error = "Restaurant id required" });
            }

            var tables = await _tableRepository.GetAllAsync(restaurantId);
            return Ok(tables.Select(b => (TableResponseDTO)b).ToList());
        }

        // GET: api/Tables/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<TableResponseDTO>> GetTableAsync(string id)
        {
            if (!await _tableRepository.IfExist(id))
            {
                return NotFound(new { Error = "Table with given id not found" });
            }

            var table = await _tableRepository.GetAsync(id);
            return Ok((TableResponseDTO)table);
        }

        // PUT: api/Tables/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<ActionResult<TableResponseDTO>> PutTableAsync(string id, TableRequestDTO tableRequest)
        {
            if (await _tableRepository.IfExist(id))
            {
                return NotFound(new { Error = "Table with given id not found" });
            }

            var oldTable = await _tableRepository.GetAsync(id);
            if (tableRequest.Restaurant != oldTable.Restaurant.Id)
            {
                return BadRequest(new { Error = "Restaurant can not be changed" });
            }

            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            if (!await _permissionValidation.isManagerTableOwnerAsync(id, currentUserEmail))
            {
                return Unauthorized(new { Error = "This table does not belong to your restaurant" });
            }

            var table = await _tableRepository.ConvertAlterTableRequest(tableRequest, id);
            if (table == null)
            {
                return NotFound(new { Error = "One of table dependencies not found" });
            }

            var updated = await _tableRepository.UpdateAsync(table);

            return Ok((TableResponseDTO)updated);
        }

        
        // POST: api/Tables
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        [HttpPost]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<ActionResult<TableResponseDTO>> PostTableAsync(TableRequestDTO tableRequest)
        {
            var table = await _tableRepository.ConvertAlterTableRequest(tableRequest, null);
            if (table == null)
            {
                return NotFound(new { Error = "One of table dependencies not found" });
            }

            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            if (!await _permissionValidation.isManagerRestaurantOwnerAsync(tableRequest.Restaurant, currentUserEmail))
            {
                return Unauthorized(new { Error = "Restaurant with given Id is not yours" });
            }

            var created = await _tableRepository.CreateAsync(table);

            return CreatedAtAction("GetTable", new { id = created.Id }, (TableResponseDTO)created);
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
                    return Unauthorized(new { Error = "This table does not belong to your restaurant" });
                }
            }
            else
            {
                return NotFound(new { Error = "Table with given id not found" });
            }

            var deleted = await _tableRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
