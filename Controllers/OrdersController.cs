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
        private readonly IOrderRepository orderRepository;
        private readonly PermissionValidation _permissionValidation;

        public OrdersController(RestaurantSystemContext context, UserManager<User> userManager)
        {
            orderRepository = new OrderRepository(context);
            _permissionValidation = new PermissionValidation(context, userManager);
        }

        // GET: api/Orders
        [HttpGet]
        [Authorize(Roles = "RestaurantManager, Customer, RestaurantEveryDayUse")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            var userRole = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Role).Value;

            if (userRole == "Customer")
            {
                //add filter to show only orders for this customer
            }

            var orders = await orderRepository.GetAllAsync();
            return Ok(orders);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        [Authorize(Roles = "RestaurantManager, Customer, RestaurantEveryDayUse")]
        public async Task<ActionResult<Order>> GetOrder(string id)
        {
            var order = await orderRepository.GetAsync(id);

            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            var userRole = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Role).Value;

            if (userRole == "Customer" && currentUserEmail != order.Customer.Email)
            {
                return BadRequest();
            }

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "RestaurantManager, Customer, RestaurantEveryDayUse")]
        public async Task<IActionResult> PutOrder(string id, OrderRequest request)
        {
            var oldOrder = await orderRepository.GetAsync(id);
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

            var order = await orderRepository.ConvertAlterOrderRequest(request);
            if (order == null)
            {
                return BadRequest();
            }

            if (await orderRepository.IfExist(id))
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

            var orderSaved = await orderRepository.UpdateAsync(order);

            if (orderSaved == null)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Order>> PostOrder(OrderRequest orderRequest)
        {
            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            var userRole = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Role).Value;

            if (userRole == "Customer")
            {
                orderRequest.Customer = currentUserEmail;
            }

            var order = await orderRepository.ConvertAlterOrderRequest(orderRequest);
            if (order == null)
            {
                return BadRequest();
            }

            if (userRole == "RestaurantManager" || userRole == "RestaurantEveryDayUse")
            {
                if (!await _permissionValidation.isManagerRestaurantOwnerAsync(orderRequest.Restaurant, currentUserEmail))
                {
                    return Unauthorized();
                }
            }
            var saved = await orderRepository.CreateAsync(order);

            if (saved == null)
            {
                return BadRequest();
            }

            return CreatedAtAction("GetOrder", new { id = saved.Id }, saved);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "RestaurantManager, Customer, RestaurantEveryDayUse")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var returnedOrder = await orderRepository.DeleteAsync(id);

            if (returnedOrder == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
