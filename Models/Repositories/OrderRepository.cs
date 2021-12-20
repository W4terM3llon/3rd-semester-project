﻿using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models.Requests;
using RestaurantSystem.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories 
{
    public class OrderRepository : IOrderRepository
    {
        private readonly RestaurantSystemContext _context;

        public OrderRepository(RestaurantSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync(string restaurantId, DateTime date, string userId)
        {
            return await _context.Order.Include(order => order.OrderLines).ThenInclude(ol => ol.Dish).ThenInclude(dish => dish.DishCategory).Include(order => order.Restaurant).ThenInclude(restaurant => restaurant.Address).Include(order => order.Customer).ThenInclude(customer => customer.Address).Include(order => order.OrderStage).Where(order=>
                    (order.Restaurant.Id == restaurantId || restaurantId == null) &&
                    ((order.Date.Year == date.Year && order.Date.Month == date.Month && order.Date.Day == date.Day) || date == DateTime.MinValue) &&
                    (order.Customer.SystemId == userId || userId == null)
                ).ToListAsync();
        }

        public async Task<Order> GetAsync(string id)
        {
            var order = await _context.Order.Include(order => order.OrderLines).ThenInclude(ol => ol.Dish).ThenInclude(dish => dish.DishCategory).Include(order => order.Restaurant).ThenInclude(restaurant => restaurant.Address).Include(order => order.Customer).ThenInclude(customer => customer.Address).Include(order => order.OrderStage).FirstOrDefaultAsync(order => order.Id == id);
            return order;
        }

        public async Task<Order> CreateAsync(Order order)
        {
            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    var orderStagePersistance = await _context.OrderStage.FirstOrDefaultAsync(os => os.Id == "1"); // Sets order status to processing !! CREATE IN DB
                    if (orderStagePersistance == null)
                    {
                        return null;
                    }
                    order.OrderStage = orderStagePersistance;
                    await _context.Order.AddAsync(order);
                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return order;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }

        public async Task<Order> DeleteAsync(string id)
        {
            if (await IfExist(id))
            {
                using (var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        var orderContext = await GetAsync(id);
                        var order = await _context.Order.FirstOrDefaultAsync(order => order.Id == id);
                        _context.Order.Remove(order);
                        await _context.SaveChangesAsync();
                        transaction.Commit();

                        return orderContext;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }
        
       

        public async Task<Order> PatchAsync(string orderId, string orderStageId)
        {
            if (await IfExist(orderId))
            {
                using (var transaction = _context.Database.BeginTransaction(IsolationLevel.RepeatableRead))
                {
                    try
                    {
                        var orderPersistance = await _context.Order.FirstOrDefaultAsync(order => order.Id == order.Id);
                        var orderStagePersistance = await _context.OrderStage.FirstOrDefaultAsync(os => os.Id == orderStageId);
                        orderPersistance.OrderStage = orderStagePersistance;

                        await _context.SaveChangesAsync();
                        transaction.Commit();

                        return orderPersistance;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }


        public async Task<bool> IfExist(string id)
        {
            return await _context.Order.AnyAsync(order => order.Id == id);
        }

        public bool ifOrderlinesUnique(List<OrderLineRequestDTO> orderLines)
        {
            List<string> dishes = new List<string>();
            foreach (var orderLine in orderLines)
            {
                if (dishes.Contains(orderLine.Dish))
                {
                    return false;
                }
                else
                {
                    dishes.Add(orderLine.Dish);
                }
            };

            return true;
        }

        public bool ifOrderlinesQuantityQuantityCorrect(List<OrderLineRequestDTO> orderLines)
        {
            foreach (var orderLine in orderLines)
            {
                if (!(orderLine.Quantity > 0 && orderLine.Quantity < 100))
                {
                    return false;
                }
            };

            return true;
        }

        public async Task<Order> ConvertAlterOrderRequest(OrderRequestDTO request, string id)
        {
            var customer = await _context.User.FirstOrDefaultAsync(customer => customer.SystemId == request.Customer);
            var restaurant = await _context.Restaurant.FirstOrDefaultAsync(restaurant => restaurant.Id == request.Restaurant);

            if (customer == null || restaurant == null)
            {
                return null;
            }

            float amount = 0;
            var orderLines = new List<OrderLine>();
            foreach (var orderLineRequest in request.OrderLines) {
                var dish = await _context.Dish.FirstOrDefaultAsync(dish => dish.Id == orderLineRequest.Dish);
                if (dish == null)
                {
                    return null;
                }
                else
                {
                    int quantity = orderLineRequest.Quantity;
                    var orderLine = new OrderLine
                    {
                        Id = IdGenerator.GenerateId(),
                        Dish = dish,
                        Quantity = quantity,
                    };
                    orderLines.Add(orderLine);
                    amount += dish.Price;
                }
            }

            var orderPlacedStage = await _context.OrderStage.FirstOrDefaultAsync(stage => stage.Name == "Processing");

            var order = new Order()
            {
                Id = id != null ? id : IdGenerator.GenerateId(),
                Date = DateTime.Now,
                OrderLines = orderLines,
                OrderStage = orderPlacedStage,
                Customer = customer,
                Restaurant = restaurant

            };
            return order;
        }

        public Task<Order> UpdateAsync(Order order)
        {
            throw new NotImplementedException();
        }
    }
}