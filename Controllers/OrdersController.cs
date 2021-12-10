﻿using System;
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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPermissionValidation _permissionValidation;
        private readonly IOrderStageRepository _orderStageRepository;

        public OrdersController(IOrderRepository orderRepository, IOrderStageRepository orderStageRepository, IPermissionValidation permissionValidation)
        {
            _orderRepository = orderRepository;
            _orderStageRepository = orderStageRepository;
            _permissionValidation = permissionValidation;
        }

        // GET: api/Orders
        [HttpGet]
        [Authorize(Roles = "RestaurantManager, Customer, RestaurantEveryDayUse")]
        public async Task<ActionResult<List<OrderResponseDTO>>> GetOrder([FromQuery] string restaurantId, [FromQuery] DateTime date, [FromQuery] string userId)
        {
            IEnumerable<Order> orders = new List<Order>();

            //Customer can not see another customers orders
            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            var userRole = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Role).Value;
            if (userRole == "Customer")
            {
                if (userId == null)
                {
                    return BadRequest(new { Error = "User id required" });
                }
                else
                {
                    if (!await _permissionValidation.isUserTheSameAsync(userId, currentUserEmail))
                    {
                        return Unauthorized(new { Error = "Can not retrieve another costomers orders" });
                    }
                    else
                    {
                        orders = await _orderRepository.GetAllAsync(null, DateTime.MinValue, userId);
                    }
                }
            }
            else if (userRole == "RestaurantManager" || userRole == "RestaurantEveryDayUse")
            {
                if (restaurantId == null || date == DateTime.MinValue)
                {
                    return BadRequest(new { Error = "RestaurantId and date required" });
                }
                else
                {
                    //Manager and RestaurantEveryDayUse can not retrieve another restaurants bookings
                    if ((userRole == "RestaurantManager" && !await _permissionValidation.isManagerRestaurantOwnerAsync(restaurantId, currentUserEmail)) ||
                        (userRole == "RestaurantEveryDayUse" && !await _permissionValidation.isEveryDayUseAccountRestaurantsOwnershipAsync(restaurantId, currentUserEmail)))
                    {

                        return Unauthorized(new { Error = "Can not retrieve orders of another restaurant" });
                    }
                    else
                    {
                        orders = await _orderRepository.GetAllAsync(restaurantId, date, null);
                    }
                }
            }

            return Ok(orders.Select(b => (OrderResponseDTO)b).ToList());
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        [Authorize(Roles = "RestaurantManager, Customer, RestaurantEveryDayUse")]
        public async Task<ActionResult<OrderResponseDTO>> GetOrder(string id)
        {
            if (!await _orderRepository.IfExist(id))
            {
                return NotFound(new { Error = "Order not found" });
            }

            var order = await _orderRepository.GetAsync(id);

            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            var userRole = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Role).Value;
            if (userRole == "Customer")
            {
                if (!await _permissionValidation.isUserTheSameAsync(order.Customer.SystemId, currentUserEmail))
                {
                    return Unauthorized(new { Error = "Can not retrieve another costomers booking" });
                }
            }
            else if (userRole == "RestaurantManager" || userRole == "RestaurantEveryDayUse")
            {
                if ((userRole == "RestaurantManager" && !await _permissionValidation.isManagerRestaurantOwnerAsync(order.Restaurant.Id, currentUserEmail)) ||
                    (userRole == "RestaurantEveryDayUse" && !await _permissionValidation.isEveryDayUseAccountRestaurantsOwnershipAsync(order.Restaurant.Id, currentUserEmail)))
                {

                    return Unauthorized(new { Error = "Can not retrieve orders of another restaurant" });
                }
            }

            return Ok((OrderResponseDTO)order);
        }

        
        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{id}")]
        [Authorize(Roles = "RestaurantManager, Customer, RestaurantEveryDayUse")]
        public async Task<ActionResult<OrderResponseDTO>> PatchOrder(string id, string orderStageId)
        {
            
            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            var userRole = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Role).Value;

            if (!await _orderRepository.IfExist(id))
            {
                return NotFound(new { Error = "Order with given id not found" });
            }
            var oldOrder = await _orderRepository.GetAsync(id);

            if (!await _orderStageRepository.IfExist(id))
            {
                return NotFound(new { Error = "Order stage with given id not found" });
            }

            if (!await _permissionValidation.isManagerRestaurantOwnerAsync(oldOrder.Restaurant.Id, currentUserEmail) && !await _permissionValidation.isEveryDayUseAccountRestaurantsOwnershipAsync(oldOrder.Restaurant.Id, currentUserEmail))
            {
                return Unauthorized(new { Error = "Can not make an order for another user" });
            }

            var orderSaved = await _orderRepository.PatchAsync(id, orderStageId);

            return Ok((OrderResponseDTO)orderSaved);
        }
        

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "RestaurantManager, Customer")]
        public async Task<ActionResult<OrderResponseDTO>> PostOrder(OrderRequestDTO orderRequest)
        {
            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            var userRole = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Role).Value;

            if (!await _permissionValidation.isUserTheSameAsync(orderRequest.Customer, currentUserEmail))
            {
                return Unauthorized(new { Error = "Can not make an order for another user" });
            }

            var order = await _orderRepository.ConvertAlterOrderRequest(orderRequest, null);
            if (order == null)
            {
                return NotFound(new { Error = "One of order dependencies not found" });
            }

            foreach (var orderLine in orderRequest.OrderLines) 
            {
                if(!await _permissionValidation.isDishRestaurantOwnershipAsync(orderLine.Dish, orderRequest.Restaurant))
                {
                    return BadRequest(new { Error = "One of chosen dishes does not belong to the chosen restaurant" });
                }
            };

            if (!_orderRepository.ifOrderlinesUnique(orderRequest.OrderLines))
            {
                return BadRequest(new { Error = "The same dish is in more that one orderline" });
            }

            if (!_orderRepository.ifOrderlinesQuantityQuantityCorrect(orderRequest.OrderLines))
            {
                return BadRequest(new { Error = "Quantity should alway be between 1 and 99" });
            }

            if (!await _orderStageRepository.IfExist("1"))
            {
                return NotFound(new { Error = "Beginning order stage with id 1 is required in the databse" });
            }

            var saved = await _orderRepository.CreateAsync(order);

            return CreatedAtAction("GetOrder", new { id = saved.Id }, (OrderResponseDTO)saved);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "RestaurantManager, Customer")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var userRole = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Role).Value;
            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            if (!await _orderRepository.IfExist(id))
            {
                return NotFound(new { Error = "Order not found" });
            }

            var order = await _orderRepository.GetAsync(id);
            if (!await _permissionValidation.isUserTheSameAsync(order.Customer.SystemId, currentUserEmail))
            {
                return Unauthorized(new { Error = "Can not delete another user's order" });
            }

            var returnedOrder = await _orderRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
