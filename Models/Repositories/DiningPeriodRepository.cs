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
        public async Task<IEnumerable<DiningPeriodRequest>> GetAllAsync()
        {
            var diningPeriods = await _context.DiningPeriod.Include(diningPeriod => diningPeriod.Restaurant).ToListAsync();
            var diningPeriodRequest = diningPeriods.ConvertAll(x => new DiningPeriodRequest { Id = x.Id, TimeStartMinutes = x.TimeStartMinutes, DurationMinutes = x.DurationMinutes, Name = x.Name, Restaurant = x.Restaurant.Id, DayOfWeek = x.DayOfWeek });
            return diningPeriodRequest;
        }
        public async Task<DiningPeriodRequest> GetAsync(string id)
        {
            if (await IfExist(id))
            {
                var diningPeriod = await _context.DiningPeriod.Include(diningPeriod => diningPeriod.Restaurant).FirstOrDefaultAsync(diningPeriod => diningPeriod.Id == id);
                var diningPeriodRequest = new DiningPeriodRequest
                {
                    Id = diningPeriod.Id,
                    TimeStartMinutes = diningPeriod.TimeStartMinutes,
                    DurationMinutes = diningPeriod.DurationMinutes,
                    Name = diningPeriod.Name,
                    Restaurant = diningPeriod.Restaurant.Id,
                    DayOfWeek = diningPeriod.DayOfWeek
                };
                return diningPeriodRequest;
            }
            else 
            {
                return null;
            }
        }
        public async Task<DiningPeriodRequest> UpdateAsync(DiningPeriodRequest diningPeriodRequest)
        {
            if (await IfExist(diningPeriodRequest.Id) || await IfPeriodsOverlap(diningPeriodRequest) || !IfDiningTimeCorrent(diningPeriodRequest))
            {
                var diningPeriod = await _context.DiningPeriod.FirstOrDefaultAsync(diningPeriod => diningPeriod.Id == diningPeriodRequest.Id);
                diningPeriod.TimeStartMinutes = diningPeriodRequest.TimeStartMinutes;
                diningPeriod.DurationMinutes = diningPeriodRequest.DurationMinutes;
                diningPeriod.Name = diningPeriodRequest.Name;
                diningPeriod.DayOfWeek = diningPeriodRequest.DayOfWeek;
                await _context.SaveChangesAsync();
                return diningPeriodRequest;
            }
            else
            {
                return null;
            }
        }
        public async Task<DiningPeriodRequest> CreateAsync(DiningPeriodRequest diningPeriodRequest)
        {
            var restaurant = await _context.Restaurant.FirstOrDefaultAsync(restaurant => restaurant.Id == diningPeriodRequest.Restaurant);

            if (restaurant == null || await IfPeriodsOverlap(diningPeriodRequest) || !IfDiningTimeCorrent(diningPeriodRequest))
            {
                return null;
            }

            var diningPeriod = new DiningPeriod
            {
                Id = new Random().Next(1, 1000).ToString(), //Replace by real id generator
                Name = diningPeriodRequest.Name,
                TimeStartMinutes = diningPeriodRequest.TimeStartMinutes,
                DurationMinutes = diningPeriodRequest.DurationMinutes,
                Restaurant = restaurant,
                DayOfWeek = diningPeriodRequest.DayOfWeek
            };

            await _context.DiningPeriod.AddAsync(diningPeriod);
            await _context.SaveChangesAsync();

            diningPeriodRequest.Id = diningPeriod.Id;
            return diningPeriodRequest;
        }

        public async Task<DiningPeriodRequest> DeleteAsync(string id)
        {
            if (await IfExist(id))
            {
                var diningPeriodRequest = await GetAsync(id);
                var diningPeriod = await _context.DiningPeriod.FirstOrDefaultAsync(diningPeriod => diningPeriod.Id == id);
                _context.DiningPeriod.Remove(diningPeriod);
                await _context.SaveChangesAsync();
                return diningPeriodRequest;
            }
            else
            {
                return null;
            }
        }

        private async Task<bool> IfExist(string id)
        {
            return await _context.DiningPeriod.AnyAsync(diningPeriod => diningPeriod.Id == id);
        }

        private async Task<bool> IfPeriodsOverlap(DiningPeriodRequest diningPeriodRequest) 
        {
            var requestPeriodStart = diningPeriodRequest.TimeStartMinutes;
            var requestPeriodEnd = (diningPeriodRequest.TimeStartMinutes + diningPeriodRequest.DurationMinutes) % (24 * 60);

            var diningPeriodRequests = await GetAllAsync();
            foreach (DiningPeriodRequest DPR in diningPeriodRequests)
            {
                int presentPeriodStart = DPR.TimeStartMinutes;
                var presentPeriodEnd = (DPR.TimeStartMinutes + DPR.DurationMinutes) % (24 * 60);

                if ((diningPeriodRequest.DayOfWeek== DPR.DayOfWeek) &&
                    ((requestPeriodStart >= presentPeriodStart && requestPeriodStart <= presentPeriodEnd) ||
                    (requestPeriodEnd >= presentPeriodStart && requestPeriodEnd <= presentPeriodEnd) ||
                    (requestPeriodStart <= presentPeriodStart && requestPeriodEnd >= presentPeriodEnd)))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IfDiningTimeCorrent(DiningPeriodRequest diningPeriodRequest) 
        {
            if (diningPeriodRequest.TimeStartMinutes > 24 * 60 || diningPeriodRequest.TimeStartMinutes < 0 || diningPeriodRequest.DurationMinutes > 24 * 60 || diningPeriodRequest.DurationMinutes <= 0 || ((int)diningPeriodRequest.DayOfWeek) < 0 || ((int)diningPeriodRequest.DayOfWeek) > 6)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
