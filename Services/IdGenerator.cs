using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Services
{
    public static class IdGenerator
    {
        public static string GenerateId() {
            string id = "";
            DateTime date = DateTime.Now;
            id += date.Date.Year % 100;
            id += date.Date.Month;
            id += date.Date.Day;
            id += date.Date.Hour;
            id += Guid.NewGuid().ToString().Substring(0, 5);
            return id;
        }
    }
}
