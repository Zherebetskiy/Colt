using Colt.Domain.Entities;
using Colt.Domain.Repositories;
using Colt.Infrastructure.Persistance;

namespace Colt.Infrastructure.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
