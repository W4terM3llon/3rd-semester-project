﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Requests
{
    public class DishCategoryRequestDTO
    {
        public string Name { get; set; }
        public string Restaurant { get; set; }
    }
}