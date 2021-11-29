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
    public class DiscountRepository : IDiscountRepository
    {

        private readonly RestaurantSystemContext _context;

        public DiscountRepository(RestaurantSystemContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Discount>> GetAllAsync()
        {
            var discountCreated = await _context.Discount.ToListAsync();
            return discountCreated;
        }
        public async Task<Discount> GetAsync(string id)
        {
            var discount = await _context.Discount.FirstOrDefaultAsync(discount => discount.Id == id);
            return discount;
        }
        public async Task<Discount> UpdateAsync(Discount newDiscountData)
        {
            if (await IfExist(newDiscountData.Id))
            {
                var contextDiscount = await GetAsync(newDiscountData.Id);
                contextDiscount.Code = newDiscountData.Code;
                contextDiscount.Type = newDiscountData.Type;
                contextDiscount.Value = newDiscountData.Value;
                contextDiscount.StartDate = newDiscountData.StartDate;
                contextDiscount.ExpiryDate = newDiscountData.ExpiryDate;
                contextDiscount.ExpiryDate = newDiscountData.ExpiryDate;
                contextDiscount.isDisposable = newDiscountData.isDisposable;
                // contextDiscount.Order.Payment = newDiscountData.Order.Payment;
                await _context.SaveChangesAsync();
                return newDiscountData;
            }
            else
            {
                return null;
            }

        }
        public async Task<Discount> CreateAsync(Discount discount)
        {
            await _context.Discount.AddAsync(discount);
            await _context.SaveChangesAsync();
            return discount;
        }
        public async Task<Discount> DeleteAsync(string id)
        {
            if (await IfExist(id))
            {
                var discount = await GetAsync(id);
                _context.Discount.Remove(discount);
                await _context.SaveChangesAsync();
                return discount;
            }
            else
            {
                return null;
            }
        }
        private async Task<bool> IfExist(string id)
        {
            return await _context.Discount.AnyAsync(Discount => Discount.Id == id);
        }

    }
}