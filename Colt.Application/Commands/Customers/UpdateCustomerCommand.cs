using Colt.Application.Common.Models;
using MediatR;

namespace Colt.Application.Commands.Customers
{
    public class UpdateCustomerCommand : IRequest<CustomerDto>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<CustomerProductDto> Products { get; set; }
    }
}
