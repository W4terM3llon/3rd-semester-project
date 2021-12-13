﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestaurantSystem.Models;
using RestaurantSystem.Models.Requests;
using RestaurantSystem.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestaurantSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public LoginController(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Actor, user.SystemId),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var accessToken = _tokenService.GenerateAccessToken(authClaims);

                return Ok(new
                {
                    token = accessToken
                });
            }
            return Unauthorized(new {Error="Wrong email or password" } );
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return BadRequest(new { Error = "User already exists!" });

            if (model.Email == "" || model.FirstName == "" || model.LastName == "" || model.Address.Street == "" || model.Address.Appartment == "" || model.PhoneNumber == "") { 
                return BadRequest(new { Error = "Empty fields not allowed"});
            }

            var address = new Address()
            {
                Id = new Random().Next(1, 1000).ToString(), //Replace by real id generator
                Street = model.Address.Street,
                Appartment = model.Address.Appartment
            };

            User user = new User()
            {
                SystemId = new Random().Next(1, 1000).ToString(), //Replace by real id generator
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = address,
                PhoneNumber = model.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors;
                var enumerator = errors.GetEnumerator();
                enumerator.MoveNext();
                var message = string.Join(", ", enumerator.Current.Description);
                return BadRequest(new { Error = message });
            }
            await _userManager.AddToRoleAsync(user, "Customer");

            return CreatedAtAction("Login", new { id = user.SystemId }, new { Success="User registered" });
        }
    }
}
