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
    public class DiningPeriodsController : ControllerBase
    {
        private readonly DiningPeriodRepository _diningPeriodRepository;

        public DiningPeriodsController(RestaurantSystemContext context)
        {
            _diningPeriodRepository = new DiningPeriodRepository(context);
        }

        // GET: api/DiningPeriods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiningPeriodRequest>>> GetDiningPeriod()
        {
            var diningPeriods= await _diningPeriodRepository.GetAllAsync();
            return Ok(diningPeriods);
        }

        // GET: api/DiningPeriods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DiningPeriodRequest>> GetDiningPeriod(string id)
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
        public async Task<IActionResult> PutDiningPeriod(string id, DiningPeriodRequest diningPeriodRequest)
        {
            if (id != diningPeriodRequest.Id)
            {
                return BadRequest();
            }

            var diningPeriod = await _diningPeriodRepository.UpdateAsync(diningPeriodRequest);

            if (diningPeriod == null)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/DiningPeriods
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DiningPeriod>> PostDiningPeriod(DiningPeriodRequest diningPeriodRequest)
        {
            var diningPeriod = await _diningPeriodRepository.CreateAsync(diningPeriodRequest);

            if (diningPeriod == null)
            {
                return BadRequest();
            }

            return CreatedAtAction("GetDiningPeriod", new { id = diningPeriod.Id }, diningPeriod);
        }

        // DELETE: api/DiningPeriods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiningPeriod(string id)
        {
            var returnedRestaurant = await _diningPeriodRepository.DeleteAsync(id);

            if (returnedRestaurant == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}