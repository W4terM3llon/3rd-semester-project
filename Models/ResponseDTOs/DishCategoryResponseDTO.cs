using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class DishCategoryResponseDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public static explicit operator DishCategoryResponseDTO(DishCategory dishCategory)
        {
            if (dishCategory == null)
            {
                return null;
            }
            else
            {

                return new DishCategoryResponseDTO()
                {
                    Id = dishCategory.Id,
                    Name = dishCategory.Name
                };
            }
        }
    }
}
