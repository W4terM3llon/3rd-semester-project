using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models.Requests;
using RestaurantSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    public class DishCategoryRepository : IDishCategoryRepository
    {
        private readonly RestaurantSystemContext _context;

        public DishCategoryRepository(RestaurantSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DishCategory>> GetAllAsync()
        {
            var dishCategories = await _context.DishCategory.ToListAsync();
            return dishCategories;
        }

        public async Task<DishCategory> GetAsync(string id)
        {
            if (await IfExist(id))
            {
                var dishCategory = await _context.DishCategory.FirstOrDefaultAsync(dishCategory => dishCategory.Id == id);
                return dishCategory;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> IfExist(string id)
        {
            return await _context.DishCategory.AnyAsync(dishCategory => dishCategory.Id == id);
        }

        public async Task<DishCategory> ConvertAlterDishRequest(DishCategoryRequestDTO request)
        {
            var restaurant = await _context.Restaurant.Include(restaurant => restaurant.Manager).FirstOrDefaultAsync(restaurant => restaurant.Id == request.Restaurant);

            if (restaurant == null)
            {
                return null;
            }

            var dishCategory = new DishCategory()
            {
                Id = IdGenerator.GenerateId(),
                Name = request.Name
            };
            return dishCategory;
        }
    }
}
