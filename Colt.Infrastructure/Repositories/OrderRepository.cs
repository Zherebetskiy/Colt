using Colt.Domain.Entities;
using Colt.Domain.Repositories;
using Colt.Infrastructure.Persistance;

namespace Colt.Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
