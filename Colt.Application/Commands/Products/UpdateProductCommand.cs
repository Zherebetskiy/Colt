using AutoMapper;
using Colt.Application.Common.Models;
using Colt.Domain.Repositories;
using MediatR;

namespace Colt.Application.Commands.Products
{
    public class UpdateProductCommand : IRequest<ProductDto>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(
            IProductRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (product == null)
            {
                throw new ArgumentNullException("Product not found");
            }

            product.Name = request.Name;

            await _repository.UpdateAsync(product, cancellationToken);

            return _mapper.Map<ProductDto>(product);
        }
    }
}
