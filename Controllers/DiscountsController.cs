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

    //there is no validation
    //neither tested not reviewed

    /*
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly DiscountRepository _discountRepository;

        public DiscountsController(RestaurantSystemContext context)
        {
            _discountRepository = new DiscountRepository(context);
        }

        // GET: api/Discounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Discount>>> GetDiscountsAsync()
        {
            var discounts = await _discountRepository.GetAllAsync();
            return Ok(discounts);
        }

        // GET: api/Discounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Discount>> GetDiscountAsync(string id)
        {
            var discount = await _discountRepository.GetAsync(id);

            if (discount == null)
            {
                return NotFound();
            }

            return Ok(discount);
        }

        // PUT: api/Discounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDiscount(string id, Discount discount)
        {
            if (id != discount.Id)
            {
                return BadRequest();
            }

            var returnedDiscount = await _discountRepository.UpdateAsync(discount);

            if (returnedDiscount == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Discounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Discount>> PostDiscountAsync(Discount discount)
        {

            discount.Id = new Random().Next(1, 1000).ToString(); //Replace by real id generator
                                                                 // discount.Order.Id = new Random().Next(1, 1000).ToString(); //Replace by real id generator

            await _discountRepository.CreateAsync(discount);

            return CreatedAtAction("GetDiscount", new { id = discount.Id }, discount);
        }

        // DELETE: api/Discounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscount(string id)
        {
            var returnedDiscount = await _discountRepository.DeleteAsync(id);

            if (returnedDiscount == null)
            {
                return NotFound();
            }

            return NoContent();
        }
}*/
}