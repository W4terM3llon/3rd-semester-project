using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models.Requests;
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
        /*
        public async Task<DishCategory> CreateAsync(DishCategory dishCategory)
        {
            var restaurant = await _context.Restaurant.FirstOrDefaultAsync(restaurant => restaurant.Id == dishCategory.Restaurant.Id);
            if (restaurant == null)
            {
                return null;
            }

            await _context.DishCategory.AddAsync(dishCategory);
            await _context.SaveChangesAsync();

            return dishCategory;
        }

        public async Task<DishCategory> DeleteAsync(string id)
        {
            if (await IfExist(id))
            {
                var dishCategoryContext = await GetAsync(id);
                var dishCategory = await _context.DishCategory.FirstOrDefaultAsync(dishCategory => dishCategory.Id == id);
                _context.DishCategory.Remove(dishCategory);
                await _context.SaveChangesAsync();
                return dishCategoryContext;
            }
            else
            {
                return null;
            }
        }*/

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
        /*
        public async Task<DishCategory> UpdateAsync(DishCategory request)
        {
            if (await IfExist(request.Id))
            {
                var dish = await GetAsync(request.Id);

                dish.Name = request.Name;

                await _context.SaveChangesAsync();
                return dish;
            }
            else
            {
                return null;
            }
        }*/

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
                Id = new Random().Next(1, 1000).ToString(),
                Name = request.Name
            };
            return dishCategory;
        }
    }
}
