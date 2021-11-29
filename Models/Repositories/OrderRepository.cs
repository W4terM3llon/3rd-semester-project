using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models.Requests;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Order.Include(order => order.Discount).Include(order => order.Payment).
                Include(order => order.OrderStage).Include(order => order.OrderLines).Include(order => order.Restaurant).ToListAsync();
        }

        public async Task<Order> GetAsync(string id)
        {
            var order = await _context.Order.Include(order => order.Discount).Include(order => order.Payment).Include(order => order.OrderStage).Include(order => order.OrderLines).FirstOrDefaultAsync(order => order.Id == id);
            return order;
        }

        public async Task<Order> CreateAsync(Order order)
        {
            await _context.Order.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> DeleteAsync(string id)
        {
            if (await IfExist(id))
            {
                var orderContext = await GetAsync(id);
                var order = await _context.Order.FirstOrDefaultAsync(order => order.Id == id);
                _context.Order.Remove(order);
                await _context.SaveChangesAsync();
                return orderContext;
            }
            else
            {
                return null;
            }
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            if (await IfExist(order.Id))
            {
                var orderPersistance = await _context.Order.FirstOrDefaultAsync(order => order.Id == order.Id);

                orderPersistance.Date = order.Date;
                orderPersistance.Payment = order.Payment;
                orderPersistance.Discount = order.Discount;
                orderPersistance.OrderLines = order.OrderLines;
                orderPersistance.OrderStage = order.OrderStage;

                await _context.SaveChangesAsync();
                return orderPersistance;
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

        public async Task<Order> ConvertAlterOrderRequest(OrderRequest request)
        {
            var discount = await _context.Discount.FirstOrDefaultAsync(discount => discount.Id == request.Discount);
            var customer = await _context.Customer.FirstOrDefaultAsync(customer => customer.SystemId == request.Customer);
            var restaurant = await _context.Restaurant.FirstOrDefaultAsync(restaurant => restaurant.Id == request.Restaurant);

            if (discount == null || customer == null || restaurant == null)
            {
                return null;
            }

            float amount = 0;
            var orderLines = new List<OrderLine>();
            foreach (var orderLineRequest in request.OrderLines) {
                var dish = await _context.Dish.FirstOrDefaultAsync(dish => dish.Id == orderLineRequest.Dish);
                int quantity = orderLineRequest.Quantity;
                var orderLine = new OrderLine
                {
                    Dish = dish,
                    Quantity = quantity,
                };
                orderLines.Add(orderLine);
                amount += dish.Price;
            }

            var orderPlacedStage = await _context.OrderStage.FirstOrDefaultAsync(stage => stage.Name == "Placed");

            var payment = new Payment
            {
                Id = new Random().Next(1, 1000).ToString(),
                Amount = amount,
                IsPaid = false,
                DatePaid = new DateTime(),
            };

            var order = new Order()
            {
                Id = new Random().Next(1, 1000).ToString(),
                Date = DateTime.Now,
                Payment = payment,
                Discount = discount,
                OrderLines = orderLines,
                OrderStage = orderPlacedStage,
                Customer = customer,
                Restaurant = restaurant

            };
            return order;
        }
    }
}
