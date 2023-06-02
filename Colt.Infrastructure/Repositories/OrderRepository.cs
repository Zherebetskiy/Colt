using Colt.Domain.Common;
using Colt.Domain.Entities;
using Colt.Domain.Repositories;
using Colt.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Colt.Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly DbSet<Order> _dbSet;

        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.GetSet<Order>();
        }

        public override Task<Order> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return _dbSet
                .Where(x => x.Id == id)
                .Include(x => x.Products)
                    .ThenInclude(x => x.CustomerProduct)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> DeleteProductsAsync(List<OrderProduct> products, CancellationToken cancellationToken)
        {
            _dbContext.GetSet<OrderProduct>().RemoveRange(products);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

    }
}
