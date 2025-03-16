using E_Commerce511.DataAccess;
using E_Commerce511.Models;
using E_Commerce511.Repositories.IRepositories;

namespace E_Commerce511.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

    }
}
