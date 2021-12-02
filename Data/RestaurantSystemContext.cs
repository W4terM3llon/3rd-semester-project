using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Data
{
    public class RestaurantSystemContext : IdentityDbContext<User, UserRole, int>
    {
        public RestaurantSystemContext(DbContextOptions<RestaurantSystemContext> options) : base(options) 
        {
        }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Restaurant> Restaurant { get; set; }
        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<Table> Table { get; set; }
        public virtual DbSet<DiningPeriod> DiningPeriod { get; set; }
        public virtual DbSet<Discount> Discount { get; set; }
        public DbSet<Dish> Dish { get; set; }
        public DbSet<DishCategory> DishCategory { get; set; }
        public DbSet<RestaurantSystem.Models.Order> Order { get; set; }
        public DbSet<RestaurantSystem.Models.OrderStage> OrderStage { get; set; }
        public DbSet<RestaurantSystem.Models.Payment> Payment { get; set; }
        public DbSet<RestaurantSystem.Models.OrderLine> OrderLine { get; set; }

    }
}
