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
    public class BookingController : ControllerBase
    {
        private readonly BookingRepository _bookingRepository;
        private readonly PermissionValidation _permissionValidation;

        public BookingController(RestaurantSystemContext context, UserManager<User> userManager)
        {
            _bookingRepository = new BookingRepository(context);
            _permissionValidation = new PermissionValidation(context, userManager);
        }

        // GET: api/Booking
        [HttpGet]
        [Authorize(Roles = "RestaurantManager, Customer, RestaurantEveryDayUse")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBooking()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return Ok(bookings);
        }

        // GET: api/Booking/5
        [HttpGet("{id}")]
        [Authorize(Roles = "RestaurantManager, Customer, RestaurantEveryDayUse")]
        public async Task<ActionResult<Booking>> GetBooking(string id)
        {
            var booking = await _bookingRepository.GetAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        // PUT: api/Booking/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "RestaurantManager, Customer, RestaurantEveryDayUse")]
        public async Task<IActionResult> PutBooking(string id, BookingRequest bookingRequest)
        {
            var oldBooking = await _bookingRepository.GetAsync(id);
            if (id != bookingRequest.Id || bookingRequest.Restaurant != oldBooking.Restaurant.Id || bookingRequest.User != oldBooking.User.SystemId)
            {
                return BadRequest();
            }

            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            var userRole = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Role).Value;

            if (userRole == "Customer")
            {
                bookingRequest.User = currentUserEmail;
            }

            var booking = await _bookingRepository.ConvertAlterBookingRequest(bookingRequest);
            if (booking == null)
            {
                return BadRequest();
            }


            if (await _bookingRepository.IfExist(id))
            {
                if (userRole == "Customer")
                {
                    if (!await _permissionValidation.isCustomerBookingOwnerAsync(id, currentUserEmail))
                    {
                        return Unauthorized();
                    }
                }
                else if (userRole == "RestaurantManager" || userRole == "RestaurantEveryDayUse")
                {
                    if (!await _permissionValidation.isManagerBookingOwnerAsync(id, currentUserEmail))
                    {
                        return Unauthorized();
                    }
                }
            }
            else
            {
                return NotFound();
            }


            var updated = await _bookingRepository.UpdateAsync(booking);


            if (updated == null)
            {
                return BadRequest();
            }


            return NoContent();
        }

        // POST: api/Booking
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "RestaurantManager, Customer, RestaurantEveryDayUse")]
        public async Task<ActionResult<BookingRequest>> PostBooking(BookingRequest bookingRequest)
        {
            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            var userRole = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Role).Value;

            if (userRole == "Customer")
            {
                bookingRequest.User = currentUserEmail;
            }

            var booking = await _bookingRepository.ConvertAlterBookingRequest(bookingRequest);
            if (booking == null)
            {
                return BadRequest();
            }

            if (userRole == "RestaurantManager" || userRole == "RestaurantEveryDayUse")
            {
                if (!await _permissionValidation.isManagerRestaurantOwnerAsync(bookingRequest.Restaurant, currentUserEmail))
                {
                    return Unauthorized();
                }
            }

            var created = await _bookingRepository.CreateAsync(booking);

            if(created == null)
            {
                return BadRequest();
            }

            return CreatedAtAction("GetBooking", new { id = bookingRequest.Id }, bookingRequest);
        }

        // DELETE: api/Booking/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "RestaurantManager, Customer, RestaurantEveryDayUse")]
        public async Task<IActionResult> DeleteBooking(string id)
        {
            var userRole = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Role).Value;
            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;

            if (await _bookingRepository.IfExist(id))
            {
                if (userRole == "Customer")
                {
                    if (!await _permissionValidation.isCustomerBookingOwnerAsync(id, currentUserEmail))
                    {
                        return Unauthorized();
                    }
                }
                else if (userRole == "RestaurantManager" || userRole == "RestaurantEveryDayUse")
                {
                    if (!await _permissionValidation.isManagerBookingOwnerAsync(id, currentUserEmail))
                    {
                        return Unauthorized();
                    }
                }
            }
            else
            {
                return NotFound();
            }

            var booking = await _bookingRepository.DeleteAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
