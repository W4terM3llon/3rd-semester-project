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
        public async Task<ActionResult<IEnumerable<Booking>>> GetBooking([FromQuery] string restaurantId, [FromQuery] DateTime date, [FromQuery] string userId)
        {
            IEnumerable<Booking> bookings = new List<Booking>();

            //Customer can not retrieve another customers booking
            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            var userRole = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Role).Value;
            if (userRole == "Customer")
            {
                if (userId == null)
                {
                    return BadRequest(new { Error = "UserId required" });
                }
                else
                {
                    if (!await _permissionValidation.isUserTheSameAsync(userId, currentUserEmail))
                    {
                        return Unauthorized(new { Error = "Can not retrieve another costomers booking" });
                    }
                    else
                    {
                        bookings = await _bookingRepository.GetAllAsync(null, DateTime.MinValue, userId);
                    }
                }
            } else if (userRole == "RestaurantManager" || userRole == "RestaurantEveryDayUse") 
            {
                if (restaurantId == null || date == DateTime.MinValue)
                {
                    return BadRequest(new { Error = "RestaurantId and date required" });
                }
                else {
                    //Manager and RestaurantEveryDayUse can not retrieve another restaurants bookings
                    if ((userRole == "RestaurantManager" && !await _permissionValidation.isManagerRestaurantOwnerAsync(restaurantId, currentUserEmail)) ||
                        (userRole == "RestaurantEveryDayUse" && !await _permissionValidation.isEveryDayUseAccountRestaurantsOwnershipAsync(restaurantId, currentUserEmail)))
                    {

                        return Unauthorized(new { Error = "Can not retrieve bookings of another restaurant" });
                    }
                    else
                    {
                        bookings = await _bookingRepository.GetAllAsync(restaurantId, date, null);
                    }
                }
            }

            

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

            IEnumerable<Booking> bookings = new List<Booking>();

            //Customer can not retrieve another customers booking
            //Manager and RestaurantEveryDayUse can not retrieve another restaurants bookings
            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            var userRole = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Role).Value;
            if ((userRole == "Customer" && !await _permissionValidation.isUserTheSameAsync(booking.User.SystemId, currentUserEmail)) ||
                (userRole == "RestaurantManager" && !await _permissionValidation.isManagerRestaurantOwnerAsync(booking.Restaurant.Id, currentUserEmail)) ||
                (userRole == "RestaurantEveryDayUse") && !await _permissionValidation.isEveryDayUseAccountRestaurantsOwnershipAsync(booking.Restaurant.Id, currentUserEmail))
            {
                return Unauthorized(new
                {
                    Error = "Can not retrieve booking of another user"
                });
            }

            return Ok(booking);
        }

        // PUT: api/Booking/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "RestaurantManager, Customer")]
        public async Task<IActionResult> PutBooking(string id, BookingRequest bookingRequest)
        {
            if (!await _bookingRepository.IfExist(id))
            {
                return NotFound(new { Error = "Booking not found" });
            }

            var oldBooking = await _bookingRepository.GetAsync(id);
            if (id != bookingRequest.Id || bookingRequest.Restaurant != oldBooking.Restaurant.Id || bookingRequest.User != oldBooking.User.SystemId) // not allowed to be changed
            {
                return BadRequest(new { Error = "Id, restaurant, User can not be changed" });
            }

            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            var userRole = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Role).Value;

            if (!await _permissionValidation.isUserTheSameAsync(bookingRequest.User, currentUserEmail))
            {
                return Unauthorized(new { Error = "Can not change booking of another user" });
            }

            var booking = await _bookingRepository.ConvertAlterBookingRequest(bookingRequest);
            if (booking == null)
            {
                return NotFound(new { Error = "One of booking dependencies not found" });
            }

            if (!await _bookingRepository.IfTimeAvailable(booking))
            {
                return Conflict(new { Error = "Booking collides with other bookings in the system" });
            }

            var updated = await _bookingRepository.UpdateAsync(booking);

            return Ok(updated);
        }

        // POST: api/Booking
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "RestaurantManager, Customer")]
        public async Task<ActionResult<BookingRequest>> PostBooking(BookingRequest bookingRequest)
        {
            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            var userRole = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Role).Value;

            if (!await _permissionValidation.isUserTheSameAsync(bookingRequest.User, currentUserEmail))
            {
                return Unauthorized(new { Error = "Can make a booking for another user" });
            }

            var booking = await _bookingRepository.ConvertAlterBookingRequest(bookingRequest);
            if (booking == null)
            {
                return NotFound(new { Error = "One of booking dependencies not found" });
            }

            if (!await _bookingRepository.IfTimeAvailable(booking))
            {
                return Conflict(new { Error = "Booking collides with other bookings in the system" });
            }

            var created = await _bookingRepository.CreateAsync(booking);

            return CreatedAtAction("GetBooking", new { id = bookingRequest.Id }, bookingRequest);
        }

        // DELETE: api/Booking/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "RestaurantManager, Customer, RestaurantEveryDayUse")]
        public async Task<IActionResult> DeleteBooking(string id)
        {
            var userRole = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Role).Value;
            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;

            if (!await _bookingRepository.IfExist(id))
            {
                return NotFound(new { Error = "Booking not found" });
            }

            var booking = await _bookingRepository.GetAsync(id);

            if (!await _permissionValidation.isUserTheSameAsync(booking.User.SystemId, currentUserEmail))
            {
                return Unauthorized(new { Error = "Can not delete another user's booking" });
            }

            await _bookingRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
