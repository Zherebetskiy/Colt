using Colt.Domain.Repositories;
using MediatR;

namespace Colt.Application.Commands.Products
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _repository;

        public DeleteProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (product == null)
            {
                throw new ArgumentNullException("Product not found");
            }

            await _repository.DeleteAsync(product, cancellationToken);

            return true;
        }
    }
}

