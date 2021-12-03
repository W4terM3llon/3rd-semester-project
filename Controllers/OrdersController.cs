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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPermissionValidation _permissionValidation;

        public OrdersController(IOrderRepository orderRepository, IPermissionValidation permissionValidation)
        {
            _orderRepository = orderRepository;
            _permissionValidation = permissionValidation;
        }

        // GET: api/Orders
        [HttpGet]
        [Authorize(Roles = "RestaurantManager, Customer, RestaurantEveryDayUse")]
        public async Task<IActionResult> GetOrder([FromQuery] string restaurantId, [FromQuery] DateTime date, [FromQuery] string userId)
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

            return Ok(orders);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        [Authorize(Roles = "RestaurantManager, Customer, RestaurantEveryDayUse")]
        public async Task<ActionResult<Order>> GetOrder(string id)
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

            return Ok(order);
        }

        /*
        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "RestaurantManager, Customer, RestaurantEveryDayUse")]
        public async Task<IActionResult> PutOrder(string id, OrderRequest request)
        {
            var oldOrder = await _orderRepository.GetAsync(id);
            if (id != request.Id || oldOrder.Restaurant.Id != request.Restaurant)
            {
                return BadRequest();
            }

            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            var userRole = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Role).Value;

            if (userRole == "Customer")
            {
                request.Customer = currentUserEmail;
            }

            var order = await _orderRepository.ConvertAlterOrderRequest(request);
            if (order == null)
            {
                return BadRequest();
            }

            if (await _orderRepository.IfExist(id))
            {
                if (userRole == "Customer")
                {
                    if (!await _permissionValidation.isCustomerOrderOwnerAsync(id, currentUserEmail))
                    {
                        return Unauthorized();
                    }
                }
                else if (userRole == "RestaurantManager" || userRole == "RestaurantEveryDayUse")
                {
                    if (!await _permissionValidation.isManagerOrderOwnerAsync(id, currentUserEmail))
                    {
                        return Unauthorized();
                    }
                }
            }
            else
            {
                return NotFound();
            }

            var orderSaved = await _orderRepository.UpdateAsync(order);

            if (orderSaved == null)
            {
                return BadRequest();
            }

            return NoContent();
        }
        */

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "RestaurantManager, Customer")]
        public async Task<ActionResult<Order>> PostOrder(OrderRequest orderRequest)
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

            var saved = await _orderRepository.CreateAsync(order);

            return CreatedAtAction("GetOrder", new { id = saved.Id }, saved);
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
