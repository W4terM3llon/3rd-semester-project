using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant_system_new.Models.Repositories
{
    interface ITableFreePeriodsRepository
    {
        public Task<IEnumerable> GetAllAsync(string restaurant, DateTime date);
    }
}
