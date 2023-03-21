using AutoMapper;
using Colt.Application.Common.Models;
using Colt.Domain.Common;
using Colt.Domain.Entities;
using MediatR;

namespace Colt.Application.Commands
{
    public class CreateCustomerCommand : IRequest<CustomerDto>
    {
        public string Name { get; set; }
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                Name = request.Name
            };

            await _context.Customers.AddAsync(customer, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CustomerDto>(customer);
        }
    }
}
