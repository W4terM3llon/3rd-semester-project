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
        private readonly IPermissionValidation _permissionValidation;

        public UsersController(IUserRepository userRepository, IPermissionValidation permissionValidation)
        {
            _userRepository = userRepository;
            _permissionValidation = permissionValidation;
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

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize(Roles = "RestaurantManager, RestaurantEveryDayUse, Customer")]
        public async Task<ActionResult<UserResponseDTO>> GetUserAsync(string id)
        {
            if (!await _userRepository.IfExist(id))
            {
                return NotFound(new { Error = "User not found" });
            }
            var user = await _userRepository.GetAsync(id);

            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            var userRole = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Role).Value;
            if (!await _permissionValidation.isUserTheSameAsync(id, currentUserEmail))
            {
                return Unauthorized(new { Error = "Can not retrieve another users data" });
            }

            return Ok((UserResponseDTO)user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Customer, RestaurantManager, RestaurantEveryDayUse")]
        public async Task<ActionResult<UserResponseDTO>> PutUserAsync(string id, UserRequestDTO request)
        {
            if (!await _userRepository.IfExist(id))
            {
                return NotFound(new { Error = "User not found" });
            }

            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            if (!await _permissionValidation.isUserTheSameAsync(id, currentUserEmail))
            {
                return Unauthorized(new { Error = "Can not change another users data" });
            }

            var user = await _userRepository.ConvertAlterUserRequest(request, id);

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

            var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.Email).Value;
            if (!await _permissionValidation.isUserTheSameAsync(id, currentUserEmail))
            {
                return Unauthorized(new { Error = "Can not delete another users data" });
            }

            await _userRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
