using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.DTO
{
   public class OrderLine
    {
        public string id { get; set; }
        public int quantity { get; set; }
        public Dish dish { get; set; }
    }
}
