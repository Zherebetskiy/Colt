using Colt.Application.Common.Models;

namespace Colt.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<List<OrderDto>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken);

        Task<List<OrderDto>> GetAsync(CancellationToken cancellationToken);

        Task<OrderDto> CreateAsync(OrderDto orderDto, CancellationToken cancellationToken);

        Task<OrderDto> UpdateAsync(OrderDto orderDto, CancellationToken cancellationToken);

        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);

        Task<OrderDto> GetByIdWithCustomerAsync(int id, CancellationToken cancellationToken);
    }
}
