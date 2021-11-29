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
using RestaurantSystem.Services;

namespace RestaurantSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository userRepository;
        private readonly PermissionValidation _permissionValidation;

        public UsersController(RestaurantSystemContext context, UserManager<User> userManager)
        {
            this.userRepository = new UserRepository(context, userManager);
            _permissionValidation = new PermissionValidation(context, userManager);
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "RestaurantManager")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersAsync()
        {
            var users = await this.userRepository.GetAllAsync();
            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize(Roles = "RestaurantManager, RestaurantEveryDayUse")]
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
        [Authorize(Roles = "Customer, RestaurantManager, RestaurantEveryDayUse")]
        public async Task<ActionResult> PutUserAsync(string id, Register register)
        {
            if (id != register.SystemId)
            {
                return BadRequest();
            }

            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            if (! await _permissionValidation.isUserTheSameAsync(id, currentUserEmail))
            {
                return Unauthorized();
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
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            if (!await _permissionValidation.isUserTheSameAsync(id, currentUserEmail))
            {
                return Unauthorized();
            }

            var returnedUser = await this.userRepository.DeleteAsync(id);
            
            if (returnedUser == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
