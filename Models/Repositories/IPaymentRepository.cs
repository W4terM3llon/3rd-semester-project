using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    interface IPaymentRepository
    {
        public Task<IEnumerable<Payment>> GetAllAsync();
        public Task<Payment> GetAsync(string id);
        public Task<Payment> CreateAsync(Payment payment);
        public Task<Payment> UpdateAsync(Payment payment);
        public Task<Payment> DeleteAsync(string id);
    }
}
