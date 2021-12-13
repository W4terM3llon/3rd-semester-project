﻿using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    public class DishRepository : IDishRepository
    {
        private readonly RestaurantSystemContext _context;

        public DishRepository(RestaurantSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dish>> GetAllAsync(string restaurantId)
        {
            var dishCreated = await _context.Dish.Include(dish => dish.DishCategory).Include(dish => dish.Restaurant).ThenInclude(restaurant => restaurant.Address).Where(dish =>
                (dish.Restaurant.Id == restaurantId || restaurantId == null)
                ).ToListAsync();
            return dishCreated;
        }

        public async Task<Dish> GetAsync(string id)
        {
            if (await IfExist(id))
            {
                var dish = await _context.Dish.Include(dish => dish.DishCategory).Include(dish => dish.Restaurant).ThenInclude(restaurant => restaurant.Address).FirstOrDefaultAsync(dish => dish.Id == id);
                return dish;
            }
            else
            {
                return null;
            }
        }
        public async Task<Dish> UpdateAsync(Dish dishRequest)
        {
            if (await IfExist(dishRequest.Id))
            {
                using (var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        var restaurant = await _context.Restaurant.FirstOrDefaultAsync(restaurant => restaurant.Id == dishRequest.Restaurant.Id);
                        var dishCategory = await _context.DishCategory.FirstOrDefaultAsync(dishCategory => dishCategory.Id == dishRequest.DishCategory.Id);

                        if (restaurant == null || dishCategory == null)
                        {
                            return null;
                        }

                        var dish = await _context.Dish.FirstOrDefaultAsync(dish => dish.Id == dishRequest.Id);

                        dish.Name = dishRequest.Name;
                        dish.Price = dishRequest.Price;
                        dish.Description = dishRequest.Description;
                        dish.DishCategory = dishCategory;
                        await _context.SaveChangesAsync();
                        transaction.Commit();

                        return dishRequest;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<Dish> IncrementLikeAsync(string dishId)
        {
            if (!await IfExist(dishId))
            {
                return null;
            }

            Dish dish = null;

            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.RepeatableRead))
            {
                dish = await GetAsync(dishId);
                dish.Likes += 1;
                await _context.SaveChangesAsync();
                transaction.Commit();
            }

            return dish;
        }

        public async Task<Dish> CreateAsync(Dish dish)
        {
            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
            {
                try
                {
                    var restaurant = await _context.Restaurant.FirstOrDefaultAsync(restaurant => restaurant.Id == dish.Restaurant.Id);
                    var dishCategory = await _context.DishCategory.FirstOrDefaultAsync(dishCategory => dishCategory.Id == dish.DishCategory.Id);

                    if (restaurant == null || dishCategory == null)
                    {
                        return null;
                    }

                    await _context.Dish.AddAsync(dish);
                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return dish;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }

        public async Task<Dish> DeleteAsync(string id)
        {
            if (await IfExist(id))
            {
                using (var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        var dishContext = await GetAsync(id);
                        var dish = await _context.Dish.FirstOrDefaultAsync(dish => dish.Id == id);
                        _context.Dish.Remove(dish);
                        await _context.SaveChangesAsync();
                        transaction.Commit();

                        return dishContext;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> IfExist(string id)
        {
            return await _context.Dish.AnyAsync(table => table.Id == id);
        }

        public async Task<Dish> ConvertAlterDishRequest(DishRequestDTO request, string id)
        {
            var restaurant = await _context.Restaurant.Include(restaurant => restaurant.Manager).FirstOrDefaultAsync(restaurant => restaurant.Id == request.Restaurant);
            var dishCategory = await _context.DishCategory.FirstOrDefaultAsync(dishCategory => dishCategory.Id == request.DishCategory);

            if (restaurant == null || dishCategory == null)
            {
                return null;
            }

            var dish = new Dish()
            {
                Id = id != null ? id : new Random().Next(1, 1000).ToString(),
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
                Restaurant = restaurant,
                DishCategory = dishCategory
            };
            return dish;
        }
    }
}
