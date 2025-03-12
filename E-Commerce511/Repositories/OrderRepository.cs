using E_Commerce511.DataAccess;
using E_Commerce511.Models;
using E_Commerce511.Repositories.IRepositories;

namespace E_Commerce511.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext dbContext;

        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

    }
}
