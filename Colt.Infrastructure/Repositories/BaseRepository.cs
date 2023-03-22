using Colt.Domain.Common;
using Colt.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Colt.Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity<int>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken token)
        {
            await _dbSet.AddAsync(entity, token);

            await _dbContext.SaveChangesAsync(token);

            return entity;
        }

        public virtual Task<List<TEntity>> GetAsync(CancellationToken token)
        {
            return _dbSet.ToListAsync(token);
        }

        public virtual async Task<TEntity> GetByIdAsync(int id, CancellationToken token)
        {
            var entity = await _dbSet.FindAsync(id, token);

            return entity;
        }

        public virtual Task<int> SaveChangesAsync(CancellationToken token)
        {
            return _dbContext.SaveChangesAsync(token);
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken token)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync(token);

            return entity;
        }
    }
}
