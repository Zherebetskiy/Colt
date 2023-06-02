using AutoMapper;
using Colt.Application.Commands.Customers;
using Colt.Application.Common.Models;
using Colt.Application.Interfaces;
using Colt.Domain.Entities;
using Colt.Domain.Repositories;

namespace Colt.Application.Common.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public CustomerService(
            ICustomerRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CustomerDto> CreateAsync(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = _mapper.Map<Customer>(request);

            await _repository.AddAsync(customer, cancellationToken);

            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<CustomerDto> UpdateAsync(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _repository.GetWithProductsAsync(request.Id, cancellationToken);

            customer.Name = request.Name;

            var productIds = request.Products
                .Where(x => x.Id.HasValue)
                .Select(x => x.ProductId)
                .ToList();

            var deletedProducts = customer
                .Products
                .Where(x => !productIds.Contains(x.ProductId))
                .ToList();

            await _repository.DeleteProductsAsync(deletedProducts, cancellationToken);

            var createdProductsDto = request.Products
                .Where(x => !x.Id.HasValue)
                .ToList();

            var productsToAdd = _mapper.Map<List<CustomerProduct>>(createdProductsDto);

            foreach (var product in productsToAdd)
            {
                customer.Products.Add(product);   
            }

            await _repository.UpdateAsync(customer, cancellationToken);

            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var customer = await _repository.GetByIdAsync(id, cancellationToken);

            if (customer == null)
            {
                throw new Exception($"Customer with id: {id} not found");
            }

            await _repository.DeleteAsync(customer, cancellationToken);

            return true;
        }
    }
}
