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

namespace RestaurantSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository userRepository;

        public UsersController(RestaurantSystemContext context, UserManager<User> userManager)
        {
            this.userRepository = new UserRepository(context, userManager);
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersAsync()
        {
            var users = await this.userRepository.GetAllAsync();
            return Ok(users);
        }

        // GET: api/Users/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserAsync(string id)
        {
            var user = await this.userRepository.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult> PutUserAsync(string id, Register register)
        {
            if (id != register.SystemId)
            {
                return BadRequest();
            }
            
            var returnedUser = await this.userRepository.UpdateAsync(register);
            
            if (returnedUser == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var returnedUser = await this.userRepository.DeleteAsync(id);
            
            if (returnedUser == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
