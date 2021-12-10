using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {

        private readonly RestaurantSystemContext _context;

        public PaymentRepository(RestaurantSystemContext context)
        {
            _context = context;
        }

        public async Task<Payment> CreateAsync(Payment payment)
        {

            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
            {
                try
                {
                    var newPayment = new Payment
                    {
                        Id = new Random().Next(1, 1000).ToString(), //Replace by real id generator
                        Amount = payment.Amount,
                        IsPaid = payment.IsPaid,
                        DatePaid = payment.DatePaid,
                    };

                    await _context.Payment.AddAsync(newPayment);
                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return payment;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }

        public async Task<Payment> DeleteAsync(string id)
        {
            if (await IfExist(id))
            {
                using (var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        var payment = await _context.Payment.FirstOrDefaultAsync(payment => payment.Id == id);
                        _context.Payment.Remove(payment);
                        await _context.SaveChangesAsync();
                        transaction.Commit();

                        return payment;
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

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            var payments = await _context.Payment.ToListAsync();
            return payments;
        }

        public async Task<Payment> GetAsync(string id)
        {
            if (await IfExist(id))
            {
                var payment = await _context.Payment.FirstOrDefaultAsync(payment => payment.Id == id);
                return payment;
            }
            else
            {
                return null;
            }
        }

        public async Task<Payment> UpdateAsync(Payment payment)
        {
            if (await IfExist(payment.Id))
            {
                using (var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        var contextPayment = await _context.Payment.FirstOrDefaultAsync(contextPayment => contextPayment.Id == payment.Id);

                        contextPayment.Amount = payment.Amount;
                        contextPayment.DatePaid = payment.DatePaid;
                        contextPayment.IsPaid = payment.IsPaid;
                        await _context.SaveChangesAsync();
                        transaction.Commit();

                        return payment;
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
            return await _context.Payment.AnyAsync(payment => payment.Id == id);
        }
    }
}
