using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.DTO
{
    public class Dish
    {
        public string id { get; set; }
        public string name { get; set; }
        public float price { get; set; }
        public int likes { get; set; }
        public string description { get; set; }
        public DishCategory dishCategory { get; set; }
        public Restaurant restaurant { get; set; }

    }
}
