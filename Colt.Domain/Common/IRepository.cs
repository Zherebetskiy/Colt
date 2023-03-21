namespace Colt.Domain.Common
{
    public interface IRepository<TEntity> where TEntity : BaseEntity<int>
    {
        Task<TEntity> GetByIdAsync(int id);
    }
}
