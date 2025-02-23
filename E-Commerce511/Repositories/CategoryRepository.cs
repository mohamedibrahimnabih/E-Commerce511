using E_Commerce511.Models;
using E_Commerce511.Repositories.IRepositories;

namespace E_Commerce511.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public new void Edit(Category entity)
        {
            
        }
    }
}
