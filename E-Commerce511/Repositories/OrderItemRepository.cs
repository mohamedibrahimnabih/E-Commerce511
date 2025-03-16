using E_Commerce511.DataAccess;
using E_Commerce511.Models;
using E_Commerce511.Repositories.IRepositories;

namespace E_Commerce511.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        private readonly ApplicationDbContext dbContext;

        public OrderItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateRange(IEnumerable<OrderItem> orderItems)
        {
            dbContext.AddRange(orderItems);
        }
    }
}
