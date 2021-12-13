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

namespace RestaurantSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStagesController : ControllerBase
    {
        private readonly IOrderStageRepository _orderStageRepository;

        public OrderStagesController(IOrderStageRepository orderStageRepository)
        {
            _orderStageRepository = orderStageRepository;
        }

        // GET: api/OrderStages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderStageResponseDTO>>> GetOrderStage()
        {
            var orderStages = await _orderStageRepository.GetAllAsync();
            return Ok(orderStages.Select(b => (OrderStageResponseDTO)b).ToList());
        }
    }
}
