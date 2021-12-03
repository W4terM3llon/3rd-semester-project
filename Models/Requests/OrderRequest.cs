using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Requests
{
    public class OrderRequest
    {
        //public string Discount { get; set; }
        public List<OrderLineRequest> OrderLines { get; set; }
        public string Customer { get; set; }
        public string Restaurant { get; set; }
    }
}
