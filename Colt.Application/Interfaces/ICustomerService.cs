using Colt.Application.Commands.Customers;
using Colt.Application.Common.Models;

namespace Colt.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerDto> CreateAsync(CreateCustomerCommand request, CancellationToken cancellationToken);

        Task<CustomerDto> UpdateAsync(UpdateCustomerCommand request, CancellationToken cancellationToken);

        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
