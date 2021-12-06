using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    public interface ITableFreePeriodsRepository
    {
        public Task<IEnumerable> GetAllAsync(string restaurant, DateTime date);
    }
}
