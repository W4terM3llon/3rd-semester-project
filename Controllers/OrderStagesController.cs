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

        // GET: api/OrderStages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderStageResponseDTO>> GetOrderStage(string id)
        {
            if (!await _orderStageRepository.IfExist(id))
            {
                return NotFound(new { Error = "Order stage with given id not found" });
            }
            var orderStage = await _orderStageRepository.GetAsync(id);

            return (OrderStageResponseDTO)orderStage;
        }


    }
}
