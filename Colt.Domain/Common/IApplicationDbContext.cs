using Colt.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Colt.Domain.Common
{
    public interface IApplicationDbContext
    {
        DbSet<Customer> Customers { get; set; }

        DbSet<Product> Products { get; set; }

        DbSet<CustomerProduct> CustomerProducts { get; set; }

        DbSet<Order> Orders { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
