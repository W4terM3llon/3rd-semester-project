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
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository paymentRepository;

        public PaymentsController(RestaurantSystemContext context)
        {
            paymentRepository = new PaymentRepository(context);
        }

        // GET: api/Payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayment()
        {
            var payments = await paymentRepository.GetAllAsync();
            return Ok(payments);
        }

        // GET: api/Payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(string id)
        {
            var payment = await paymentRepository.GetAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        // PUT: api/Payments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(string id, Payment payment)
        {
            if (id != payment.Id)
            {
                return BadRequest();
            }

            var updatePayment = await paymentRepository.UpdateAsync(payment);

            if (updatePayment == null)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/Payments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Payment>> PostPayment(Payment payment)
        {
            var newPayment = await paymentRepository.CreateAsync(payment);

            if (newPayment == null)
            {
                return BadRequest();
            }

            return CreatedAtAction("GetPayment", new { id = payment.Id }, payment);
        }

        // DELETE: api/Payments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(string id)
        {
            var payment = await paymentRepository.DeleteAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
