using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models;
using RestaurantSystem.Models.Repositories;
using RestaurantSystem.Models.Requests;

namespace RestaurantSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        private readonly TableRepository _tableRepository;

        public TablesController(RestaurantSystemContext context)
        {
            _tableRepository = new TableRepository(context);
        }

        // GET: api/Tables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TableRequest>>> GetTableAsync()
        {
            var restaurants = await _tableRepository.GetAllAsync();
            return Ok(restaurants);
        }

        // GET: api/Tables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TableRequest>> GetTableAsync(string id)
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
        public async Task<IActionResult> PutTableAsync(string id, TableRequest tableRequest)
        {
            if (id != tableRequest.Id)
            {
                return BadRequest();
            }

            var restaurant = await _tableRepository.UpdateAsync(tableRequest);

            if (restaurant == null)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/Tables
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TableRequest>> PostTableAsync(TableRequest tableRequest)
        {
            var table = await _tableRepository.CreateAsync(tableRequest);

            if (table == null) 
            {
                return BadRequest();
            }

            return CreatedAtAction("GetTable", new { id = table.Id }, table);
        }

        // DELETE: api/Tables/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTableAsync(string id)
        {
            var returnedRestaurant = await _tableRepository.DeleteAsync(id);

            if (returnedRestaurant == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
