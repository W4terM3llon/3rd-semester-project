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
        private readonly IBookingRepository _bookingRepository;
        private readonly IPermissionValidation _permissionValidation;

        public BookingController(IBookingRepository bookingRepository, IPermissionValidation permissionValidation)
        {
            _bookingRepository = bookingRepository;
            _permissionValidation = permissionValidation;
        }

        // GET: api/Booking
        [HttpGet]
        [Authorize(Roles = "RestaurantManager, Customer, RestaurantEveryDayUse")]
        public async Task<ActionResult<IEnumerable<BookingResponseDTO>>> GetBooking([FromQuery] string restaurantId, [FromQuery] DateTime date, [FromQuery] string userId)
        {
            IEnumerable<Booking> bookings = new List<Booking>();

            var currentUserSystemId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Actor).Value;
            var userRole = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Role).Value;
            if (userRole == "Customer")
            {
                if (userId == null)
                {
                    return BadRequest(new { Error = "UserId required" });
                }
                else
                {
                    if (userId != currentUserSystemId)
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
                    if ((userRole == "RestaurantManager" && !await _permissionValidation.isManagerRestaurantOwnerAsync(restaurantId, currentUserSystemId)) ||
                        (userRole == "RestaurantEveryDayUse" && !await _permissionValidation.isEveryDayUseAccountRestaurantsOwnershipAsync(restaurantId, currentUserSystemId)))
                    {

                        return Unauthorized(new { Error = "Can not retrieve bookings of another restaurant" });
                    }
                    else
                    {
                        bookings = await _bookingRepository.GetAllAsync(restaurantId, date, null);
                    }
                }
            }

            

            return Ok(bookings.Select(b => (BookingResponseDTO)b).ToList());
        }

        // PUT: api/Booking/5
        [HttpPut("{id}")]
        [Authorize(Roles = "RestaurantManager, Customer")]
        public async Task<ActionResult<BookingResponseDTO>> PutBooking(string id, BookingRequestDTO bookingRequest)
        {
            if (!await _bookingRepository.IfExist(id))
            {
                return NotFound(new { Error = "Booking not found" });
            }

            var oldBooking = await _bookingRepository.GetAsync(id);
            if (bookingRequest.Restaurant != oldBooking.Restaurant.Id || bookingRequest.User != oldBooking.User.SystemId)
            {
                return BadRequest(new { Error = "Restaurant, User can not be changed" });
            }

            var currentUserSystemId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Actor).Value;
            var userRole = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Role).Value;

            if (bookingRequest.User != currentUserSystemId)
            {
                return Unauthorized(new { Error = "Can not change booking of another user" });
            }

            var booking = await _bookingRepository.ConvertAlterBookingRequest(bookingRequest, id);
            if (booking == null)
            {
                return NotFound(new { Error = "One of booking dependencies not found" });
            }

            if (!await _permissionValidation.isTableRestaurantOwnershipAsync(bookingRequest.Table, bookingRequest.Restaurant))
            {
                return BadRequest(new { Error = "Chosen table does not belong to the chosen restaurant" });
            }

            if (!await _permissionValidation.isDiningPeriodRestaurantOwnershipAsync(bookingRequest.DiningPeriod, bookingRequest.Restaurant))
            {
                return BadRequest(new { Error = "Chosen dining period does not belong to the chosen restaurant" });
            }

            Booking updated = null;
            try
            {
                updated = await _bookingRepository.UpdateAsync(booking);

            }
            catch (DbUpdateException ex)
            {
                return (BadRequest(new { Error = "Booking collides with another booking in the system" }));
            }

            return Ok((BookingResponseDTO)updated);
        }

        // POST: api/Booking
        [HttpPost]
        [Authorize(Roles = "RestaurantManager, Customer")]
        public async Task<ActionResult<BookingResponseDTO>> PostBooking(BookingRequestDTO bookingRequest)
        {
            var currentUserSystemId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Actor).Value;
            var userRole = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Role).Value;

            if (bookingRequest.User != currentUserSystemId)
            {
                return Unauthorized(new { Error = "Can not make a booking for another user" });
            }

            var booking = await _bookingRepository.ConvertAlterBookingRequest(bookingRequest, null);
            if (booking == null)
            {
                return NotFound(new { Error = "One of booking dependencies not found" });
            }

            if (!await _permissionValidation.isTableRestaurantOwnershipAsync(bookingRequest.Table, bookingRequest.Restaurant))
            {
                return BadRequest(new { Error = "Chosen table does not belong to the chosen restaurant" });
            }

            if (!await _permissionValidation.isDiningPeriodRestaurantOwnershipAsync(bookingRequest.DiningPeriod, bookingRequest.Restaurant))
            {
                return BadRequest(new { Error = "Chosen dining period does not belong to the chosen restaurant" });
            }

            Booking created = null;
            try
            {
                created = await _bookingRepository.CreateAsync(booking);

            }
            catch (DbUpdateException ex)
            {
                return (BadRequest(new { Error = "Booking collides with another booking in the system" }));
            }

            return CreatedAtAction("GetBooking", new { id = created.Id }, (BookingResponseDTO)created);
        }

        // DELETE: api/Booking/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "RestaurantManager, Customer, RestaurantEveryDayUse")]
        public async Task<IActionResult> DeleteBooking(string id)
        {
            var userRole = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Role).Value;
            var currentUserSystemId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Actor).Value;

            if (!await _bookingRepository.IfExist(id))
            {
                return NotFound(new { Error = "Booking not found" });
            }

            var booking = await _bookingRepository.GetAsync(id);

            if (booking.User.SystemId !=currentUserSystemId)
            {
                return Unauthorized(new { Error = "Can not delete another user's booking" });
            }

            await _bookingRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
