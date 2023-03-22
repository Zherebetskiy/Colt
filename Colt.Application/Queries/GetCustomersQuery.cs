using AutoMapper;
using Colt.Application.Common.Models;
using Colt.Domain.Repositories;
using MediatR;

namespace Colt.Application.Queries
{
    public class GetCustomersQuery : IRequest<List<CustomerDto>>
    {
    }

    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, List<CustomerDto>>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public GetCustomersQueryHandler(
            ICustomerRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _repository.GetAsync(cancellationToken);

            return _mapper.Map<List<CustomerDto>>(customers);
        }
    }
}
