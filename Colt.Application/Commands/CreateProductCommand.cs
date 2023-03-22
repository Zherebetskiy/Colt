using AutoMapper;
using Colt.Application.Common.Models;
using Colt.Domain.Entities;
using Colt.Domain.Repositories;
using MediatR;

namespace Colt.Application.Commands
{
    public class CreateProductCommand : IRequest<ProductDto>
    {
        public string Name { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(
            IProductRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name
            };

            await _repository.AddAsync(product, cancellationToken);

            return _mapper.Map<ProductDto>(product);
        }
    }
}
