using Colt.Application.Common.Models;

namespace Colt.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerProductDto>> GetProductsAsync(int id, CancellationToken cancellationToken);

        Task<CustomerDto> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<List<CustomerDto>> GetAsync(CancellationToken cancellationToken);

        Task<CustomerDto> CreateAsync(CustomerDto request, CancellationToken cancellationToken);

        Task<CustomerDto> UpdateAsync(CustomerDto request, CancellationToken cancellationToken);

        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
