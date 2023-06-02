using AutoMapper;
using Colt.Application.Common.Models;
using Colt.Application.Interfaces;
using MediatR;

namespace Colt.Application.Commands.Customers
{
    public class CreateCustomerCommand : IRequest<CustomerDto>
    {
        public string Name { get; set; }

        public Dictionary<int, decimal> Products { get; set; }
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            return _customerService.CreateAsync(request, cancellationToken);
        }
    }
}
