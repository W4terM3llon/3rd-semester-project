using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    public class DiningPeriodRepository : IDiningPeriodRepository
    {
        private readonly RestaurantSystemContext _context;

        public DiningPeriodRepository(RestaurantSystemContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<DiningPeriod>> GetAllAsync(string restaurantId)
        {
            var diningPeriods = await _context.DiningPeriod.Include(diningPeriod => diningPeriod.Restaurant).Where(diningPeriod=>
                (diningPeriod.Restaurant.Id == restaurantId || restaurantId == null)
            ).ToListAsync();
            return diningPeriods;
        }
        public async Task<DiningPeriod> GetAsync(string id)
        {
            if (await IfExist(id))
            {
                var diningPeriod = await _context.DiningPeriod.Include(diningPeriod => diningPeriod.Restaurant).FirstOrDefaultAsync(diningPeriod => diningPeriod.Id == id);
                return diningPeriod;
            }
            else
            {
                return null;
            }
        }
        public async Task<DiningPeriod> UpdateAsync(DiningPeriod diningPeriod)
        {
            if (!await IfExist(diningPeriod.Id) || await IfPeriodsOverlap(diningPeriod) || !IfDiningTimeCorrent(diningPeriod))
            {
                return null;
            }
            var diningPeriodContext = await _context.DiningPeriod.FirstOrDefaultAsync(dp => dp.Id == diningPeriod.Id);
            diningPeriodContext.TimeStartMinutes = diningPeriod.TimeStartMinutes;
            diningPeriodContext.DurationMinutes = diningPeriod.DurationMinutes;
            diningPeriodContext.Name = diningPeriod.Name;
            await _context.SaveChangesAsync();
            return diningPeriodContext;
        }
        public async Task<DiningPeriod> CreateAsync(DiningPeriod diningPeriod)
        {
            var restaurant = await _context.Restaurant.FirstOrDefaultAsync(restaurant => restaurant.Id == diningPeriod.Restaurant.Id);

            if (restaurant == null || await IfPeriodsOverlap(diningPeriod) || !IfDiningTimeCorrent(diningPeriod))
            {
                return null;
            }
            await _context.DiningPeriod.AddAsync(diningPeriod);
            await _context.SaveChangesAsync();
            return diningPeriod;
        }

        public async Task<DiningPeriod> DeleteAsync(string id)
        {
            if (await IfExist(id))
            {
                var diningPeriod = await _context.DiningPeriod.FirstOrDefaultAsync(diningPeriod => diningPeriod.Id == id);

                _context.DiningPeriod.Remove(diningPeriod);
                await _context.SaveChangesAsync();
                return diningPeriod;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> IfExist(string id)
        {
            return await _context.DiningPeriod.AnyAsync(diningPeriod => diningPeriod.Id == id);
        }

        public async Task<bool> IfPeriodsOverlap(DiningPeriod diningPeriod) 
        {
            var requestPeriodStart = diningPeriod.TimeStartMinutes;
            var requestPeriodEnd = (diningPeriod.TimeStartMinutes + diningPeriod.DurationMinutes) % (24 * 60);

            var diningPeriods = await _context.DiningPeriod.Include(diningPeriod => diningPeriod.Restaurant).Where(dp => dp.Restaurant.Id == diningPeriod.Restaurant.Id).ToListAsync();
            var currentPeriod = await _context.DiningPeriod.FirstOrDefaultAsync(dp => dp.Id == diningPeriod.Id);
            if (currentPeriod != null)
            {
                diningPeriods.Remove(currentPeriod);
            }
            foreach (DiningPeriod DPR in diningPeriods)
            {
                int presentPeriodStart = DPR.TimeStartMinutes;
                var presentPeriodEnd = (DPR.TimeStartMinutes + DPR.DurationMinutes) % (24 * 60);

                if (((requestPeriodStart >= presentPeriodStart && requestPeriodStart <= presentPeriodEnd) ||
                    (requestPeriodEnd >= presentPeriodStart && requestPeriodEnd <= presentPeriodEnd) ||
                    (requestPeriodStart <= presentPeriodStart && requestPeriodEnd >= presentPeriodEnd)))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IfDiningTimeCorrent(DiningPeriod diningPeriod) 
        {
            if (diningPeriod.TimeStartMinutes > 24 * 60 || diningPeriod.TimeStartMinutes < 0 || diningPeriod.DurationMinutes > 24 * 60 || diningPeriod.DurationMinutes <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<DiningPeriod> ConvertAlterDiningPeriodRequest(DiningPeriodRequest request, string id)
        {
            var restaurant = await _context.Restaurant.Include(restaurant => restaurant.Manager).FirstOrDefaultAsync(restaurant => restaurant.Id == request.Restaurant);
            if (restaurant == null)
            {
                return null;
            }

            var diningPeriod = new DiningPeriod()
            {
                Id = id != null ? id : new Random().Next(1, 1000).ToString(),
                Name = request.Name,
                TimeStartMinutes = request.TimeStartMinutes,
                DurationMinutes = request.DurationMinutes,
                Restaurant = restaurant
            };
            return diningPeriod;
        }
    }
}
