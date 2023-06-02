using AutoMapper;
using Colt.Application.Common.Models;
using Colt.Application.Interfaces;
using MediatR;

namespace Colt.Application.Commands.Customers
{
    public class UpdateCustomerCommand : IRequest<CustomerDto>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<CustomerProductDto> Products { get; set; }

        public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomerDto>
        {
            private readonly ICustomerService _customerService;
            private readonly IMapper _mapper;

            public UpdateCustomerCommandHandler(ICustomerService customerService)
            {
                _customerService = customerService;
            }

            public Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
            {
                return _customerService.UpdateAsync(request, cancellationToken);
            }
        }
    }
}
