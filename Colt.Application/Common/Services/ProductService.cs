using AutoMapper;
using Colt.Application.Common.Exceptions;
using Colt.Application.Common.Models;
using Colt.Application.Interfaces;
using Colt.Domain.Entities;
using Colt.Domain.Repositories;

namespace Colt.Application.Common.Services
{
    public class ProductService : IProductService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IMapper mapper)
        {
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(id, cancellationToken);

            if (product is null)
            {
                throw new ValidationException("Product not found");
            }

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<List<ProductDto>> GetAsync(CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAsync(cancellationToken);

            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto> CreateAsync(ProductDto productDto, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(productDto);

            await _productRepository.AddAsync(product, cancellationToken);

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> UpdateAsync(ProductDto productDto, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(productDto);

            await _productRepository.UpdateAsync(product, cancellationToken);

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(id, cancellationToken);

            if (product is null)
            {
                throw new ValidationException("Product not found");
            }

            var products = await _customerRepository.GetProductsByIdAsync(id, cancellationToken);

            if (products.Any())
            {
                throw new ValidationException("Products used in customer");
            }

            await _productRepository.DeleteAsync(product, cancellationToken);

            return true;
        }
    }
}
