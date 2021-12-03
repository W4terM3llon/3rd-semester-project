using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant_system_new.Models.Repositories;
using RestaurantSystem.Data;
using RestaurantSystem.Models.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant_system_new.Controllers
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
        public async Task<ActionResult<IEnumerable>> GetDish([FromQuery] string restaurant, [FromQuery] DateTime date)
        {
            if (restaurant == null || date == DateTime.MinValue) {
                return BadRequest(new { Error = "Restaurant and date are required" });
            }

            var dishes = await _tableFreePeriodsRepository.GetAllAsync(restaurant, date);
            return Ok(dishes);
        }
    }
}
