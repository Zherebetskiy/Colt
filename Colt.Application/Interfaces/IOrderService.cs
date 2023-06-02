using Colt.Application.Common.Models;

namespace Colt.Application.Interfaces
{
    public interface IOrderService
    {
        Task<CustomerDto> CreateAsync(OrderDto orderDto, CancellationToken cancellationToken);
    }
}
