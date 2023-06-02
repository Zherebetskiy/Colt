using Colt.Application.Common.Models;

namespace Colt.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateAsync(OrderDto orderDto, CancellationToken cancellationToken);

        Task<OrderDto> UpdateAsync(OrderDto orderDto, CancellationToken cancellationToken);
    }
}
