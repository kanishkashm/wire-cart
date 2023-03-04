using Microsoft.EntityFrameworkCore;
using WireCart.Data;
using WireCart.Entities;
using WireCart.Repositories.Interfaces;

namespace WireCart.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        protected readonly WireCartDataContext _dbContext;

        public OrderRepository(WireCartDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> CheckOut(Order order)
        {
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            var orderList = await _dbContext.Orders
                            .Where(o => o.UserName == userName)
                            .ToListAsync();

            return orderList;
        }
    }
}
