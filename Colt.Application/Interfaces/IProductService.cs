using Colt.Application.Common.Models;

namespace Colt.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<List<ProductDto>> GetAsync(CancellationToken cancellationToken);

        Task<ProductDto> CreateAsync(ProductDto productDto, CancellationToken cancellationToken);

        Task<ProductDto> UpdateAsync(ProductDto productDto, CancellationToken cancellationToken);

        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
