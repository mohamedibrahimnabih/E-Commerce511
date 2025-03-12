using E_Commerce511.DataAccess;
using E_Commerce511.Models;
using E_Commerce511.Repositories.IRepositories;

namespace E_Commerce511.Repositories
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext applicationDb) : base(applicationDb)
        {
        }
    }
}
