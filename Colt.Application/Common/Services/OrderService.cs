using AutoMapper;
using Colt.Application.Common.Models;
using Colt.Application.Interfaces;
using Colt.Domain.Repositories;

namespace Colt.Application.Common.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<CustomerDto> CreateAsync(OrderDto orderDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
