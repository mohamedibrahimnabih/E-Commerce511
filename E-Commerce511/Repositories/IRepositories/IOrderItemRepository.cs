using E_Commerce511.Models;

namespace E_Commerce511.Repositories.IRepositories
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        public void CreateRange(IEnumerable<OrderItem> orderItems);
    }
}
