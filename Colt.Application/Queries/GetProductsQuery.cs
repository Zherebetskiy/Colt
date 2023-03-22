using AutoMapper;
using Colt.Application.Common.Models;
using Colt.Domain.Repositories;
using MediatR;

namespace Colt.Application.Queries
{
    public class GetProductsQuery : IRequest<List<ProductDto>>
    {
    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(
            IProductRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _repository.GetAsync(cancellationToken);

            return _mapper.Map<List<ProductDto>>(products);
        }
    }
}
