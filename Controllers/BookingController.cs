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
using RestaurantSystem.Models.Requests;

namespace RestaurantSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingRepository bookingRepository;

        public BookingController(RestaurantSystemContext context)
        {
            this.bookingRepository = new BookingRepository(context);
        }

        // GET: api/Booking
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBooking()
        {
            var bookings = await bookingRepository.GetAllAsync();
            return Ok(bookings);
        }

        // GET: api/Booking/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(string id)
        {
            var booking = await bookingRepository.GetAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        // PUT: api/Booking/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(string id, BookingRequest bookingRequest)
        {
            if (id != bookingRequest.Id)
            {
                return BadRequest();
            }

            var booking = await bookingRepository.UpdateAsync(bookingRequest);


            if (booking == null)
            {
                return BadRequest();
            }


            return NoContent();
        }

        // POST: api/Booking
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookingRequest>> PostBooking(BookingRequest bookingRequest)
        {
            var booking = await bookingRepository.CreateAsync(bookingRequest);

            if(booking == null)
            {
                return BadRequest();
            }

            return CreatedAtAction("GetBooking", new { id = bookingRequest.Id }, bookingRequest);
        }

        // DELETE: api/Booking/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(string id)
        {
            var booking = await bookingRepository.DeleteAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
