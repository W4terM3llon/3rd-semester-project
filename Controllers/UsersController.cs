using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_system_new.Models.Requests;
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
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //GET: api/Users
        [HttpGet]
        [Authorize(Roles = "RestaurantManager, RestaurantEveryDayUse, Customer")]
        public async Task<ActionResult<UserResponseDTO>> GetUsersAsync()
        {
            var currentUserSystemId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Actor).Value;
            var user = await _userRepository.GetAsync(currentUserSystemId);
            return Ok((UserResponseDTO)user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Customer, RestaurantManager, RestaurantEveryDayUse")]
        public async Task<ActionResult<UserResponseDTO>> PutUserAsync(string id, UserRequestDTO request)
        {
            if (!await _userRepository.IfExist(id))
            {
                return NotFound(new { Error = "User not found" });
            }

            var currentUserSystemId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Actor).Value;
            if (id != currentUserSystemId)
            {
                return Unauthorized(new { Error = "Can not change another users data" });
            }

            var user = _userRepository.ConvertAlterUserRequest(request, id);

            var updated = await _userRepository.UpdateAsync(user);

            return Ok((UserResponseDTO)updated);
        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (!await _userRepository.IfExist(id))
            {
                return NotFound(new { Error = "User not found" });
            }

            var currentUserSystemId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Actor).Value;
            if (id != currentUserSystemId)
            {
                return Unauthorized(new { Error = "Can not delete another users data" });
            }

            await _userRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
