using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantSystem.Data;
using RestaurantSystem.Models.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableFreePeriodsController : ControllerBase
    {
        private readonly ITableFreePeriodsRepository _tableFreePeriodsRepository;

        public TableFreePeriodsController(ITableFreePeriodsRepository tableFreePeriodsRepository)
        {
            _tableFreePeriodsRepository = tableFreePeriodsRepository;
        }

        // GET: api/Dishes
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable>> GetDish([FromQuery] string restaurantId, [FromQuery] DateTime date)
        {
            if (restaurantId == null || date == DateTime.MinValue) {
                return BadRequest(new { Error = "Restaurant and date are required" });
            }

            var tableFreePeriods = await _tableFreePeriodsRepository.GetAllAsync(restaurantId, date);
            return Ok(tableFreePeriods);
        }
    }
}
