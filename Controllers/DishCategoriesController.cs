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
    public class DishCategoriesController : ControllerBase
    {
        private readonly IDishCategoryRepository _dishCategoryRepository;

        public DishCategoriesController(IDishCategoryRepository dishCategoryRepository)
        {
            _dishCategoryRepository = dishCategoryRepository;
        }


        // GET: api/DishCategories
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DishCategoryResponseDTO>>> GetDishCategory()
        {
            var dishCategories = await _dishCategoryRepository.GetAllAsync();
            return Ok(dishCategories.Select(b => (DishCategoryResponseDTO)b).ToList());
        }
    }
}
