using CommonModels.Models.Order;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;

namespace OrderService.Services
{
    public interface IOrdersService
    {
        Task<Order> CreateOrder(Order order);
    }

    public class OrdersService : IOrdersService
    {
        private readonly OrderContext _context;

        public OrdersService(OrderContext context)
        {
            this._context = context;
        }
        public async Task<Order> CreateOrder(Order order)
        {

            order.Creation_Date = DateTime.UtcNow;
            order.Update_Date = DateTime.UtcNow;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }
    }
}
