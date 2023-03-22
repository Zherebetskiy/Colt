using AutoMapper;
using Colt.Application.Common.Models;
using Colt.Domain.Common;
using Colt.Domain.Entities;
using Colt.Domain.Repositories;
using MediatR;

namespace Colt.Application.Commands
{
    public class CreateCustomerCommand : IRequest<CustomerDto>
    {
        public string Name { get; set; }
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(
            ICustomerRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                Name = request.Name
            };

            await _repository.AddAsync(customer, cancellationToken);

            return _mapper.Map<CustomerDto>(customer);
        }
    }
}
