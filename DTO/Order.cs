using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.DTO
{
    public class Order
    {
        
        public string id { get; set; }
        public DateTime date { get; set; }
        public List<OrderLine> orderLines { get; set; }
        public OrderStage orderStage { get; set; }
        public User customer { get; set; }
        public Restaurant restaurant { get; set; }

    }
}
