using AutoMapper;
using Colt.Application.Common.Exceptions;
using Colt.Application.Common.Models;
using Colt.Application.Interfaces;
using Colt.Domain.Entities;
using Colt.Domain.Enums;
using Colt.Domain.Repositories;

namespace Colt.Application.Common.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<OrderDto> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(id, cancellationToken);

            if (order is null)
            {
                throw new ValidationException("Order not found");
            }

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<List<OrderDto>> GetAsync(CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetAsync(cancellationToken);

            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<OrderDto> CreateAsync(OrderDto orderDto, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(orderDto);

            var customer = await _customerRepository.GetWithProductsAsync(order.CustomerId, cancellationToken);

            double? totalWeight = null;
            decimal? totalPrice = null;

            order.Status = OrderStatus.Created;

            foreach (var product in order.Products)
            {
                var customerProduct = customer.Products.FirstOrDefault(x => x.Id == product.CustomerProductId);

                if (customerProduct == null)
                {
                    throw new ValidationException($"CustomerProduct with id: {product.CustomerProductId} not found");
                }

                if (product.ActualWeight.HasValue)
                {
                    order.Status = OrderStatus.Calculated;

                    product.TotalPrice = (decimal)product.ActualWeight.Value * customerProduct.Price;

                    totalWeight += product.ActualWeight.Value;
                    totalPrice += product.TotalPrice;
                }
            }

            order.TotalWeight = totalWeight;
            order.TotalPrice = totalPrice;

            await _orderRepository.AddAsync(order, cancellationToken);

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> UpdateAsync(OrderDto orderDto, CancellationToken cancellationToken)
        {
            if (!orderDto.Id.HasValue)
            {
                throw new ValidationException($"Order has no id");
            }

            var order = await _orderRepository.GetByIdAsync(orderDto.Id.Value, cancellationToken);

            var productIds = orderDto.Products
                .Where(x => x.Id.HasValue)
                .Select(x => x.CustomerProductId)
                .ToList();

            var deletedProducts = order
                .Products
                .Where(x => !productIds.Contains(x.CustomerProductId))
                .ToList();

            await _orderRepository.DeleteProductsAsync(deletedProducts, cancellationToken);

            var createdProductsDto = orderDto.Products
                .Where(x => !x.Id.HasValue)
                .ToList();

            var createdProducts = _mapper.Map<List<OrderProduct>>(createdProductsDto);

            createdProducts.ForEach(x => order.Products.Add(x));

            await _orderRepository.UpdateAsync(order, cancellationToken);

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(id, cancellationToken);

            if (order is null)
            {
                throw new ValidationException("Order not found");
            }

            await _orderRepository.DeleteAsync(order, cancellationToken);

            return true;
        }
    }
}
