using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data;
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
                using (var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
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
                        transaction.Commit();

                        return newDiscountData;
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
        public async Task<Discount> CreateAsync(Discount discount)
        {
            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
            {
                try
                {
                    await _context.Discount.AddAsync(discount);
                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return discount;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }
        public async Task<Discount> DeleteAsync(string id)
        {
            if (await IfExist(id))
            {
                using (var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        var discount = await GetAsync(id);
                        _context.Discount.Remove(discount);
                        await _context.SaveChangesAsync();
                        transaction.Commit();

                        return discount;
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
        private async Task<bool> IfExist(string id)
        {
            return await _context.Discount.AnyAsync(Discount => Discount.Id == id);
        }

    }
}