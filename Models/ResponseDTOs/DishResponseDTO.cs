using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class DishResponseDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Likes { get; set; }
        public string Description { get; set; }
        public DishCategoryResponseDTO DishCategory {get; set;}
        public RestaurantResponseDTO Restaurant { get; set; }

        public static explicit operator DishResponseDTO(Dish dish)
        {
            if (dish == null)
            {
                return null;
            }
            else
            {

                return new DishResponseDTO()
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price,
                    Likes = dish.Likes,
                    Description = dish.Description,
                    DishCategory = (DishCategoryResponseDTO)dish.DishCategory,
                    Restaurant = (RestaurantResponseDTO)dish.Restaurant
                };
            }
        }
    }
}