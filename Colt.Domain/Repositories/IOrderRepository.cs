﻿using Colt.Domain.Common;
using Colt.Domain.Entities;

namespace Colt.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<bool> DeleteProductsAsync(List<OrderProduct> products, CancellationToken cancellationToken);
    }
}
