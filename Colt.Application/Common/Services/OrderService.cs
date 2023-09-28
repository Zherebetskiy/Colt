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
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository orderRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
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

        public async Task<OrderDto> GetByIdWithCustomerAsync(int id, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdWithCustomerAsync(id, cancellationToken);

            if (order is null)
            {
                throw new ValidationException("Order not found");
            }

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<List<OrderDto>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByCustomerIdAsync(customerId, cancellationToken);

            return _mapper.Map< List<OrderDto>>(order);
        }

        public async Task<List<OrderDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(cancellationToken);

            return _mapper.Map<List<OrderDto>>(order);
        }

        public async Task<List<OrderDto>> GetAsync(CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetAsync(cancellationToken);

            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<OrderDto> CreateAsync(OrderDto orderDto, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(orderDto);

            order.Status = OrderStatus.Created;

            RecalculateTotals(order);

            if (order.TotalWeight.HasValue)
            {
                order.Status = OrderStatus.Calculated;
            }

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
                .Where(x => x.CustomerProductId.HasValue && !productIds.Contains(x.CustomerProductId.Value))
                .ToList();

            await _orderRepository.DeleteProductsAsync(deletedProducts, cancellationToken);

            order.Products = order.Products
                .Where(x => !deletedProducts.Contains(x))
                .ToList();

            var createdProductsDto = orderDto.Products
                .Where(x => !x.Id.HasValue)
                .ToList();

            var createdProducts = _mapper.Map<List<OrderProduct>>(createdProductsDto);

            createdProducts.ForEach(x => order.Products.Add(x));

            var updatedProductsDto = orderDto.Products
                .Where(x => x.Id.HasValue)
                .ToList();

            foreach (var updatedProduct in updatedProductsDto)
            {
                var product = order.Products.First(x => x.Id == updatedProduct.Id);

                product.OrderedWeight = updatedProduct.OrderedWeight;
                product.ActualWeight = updatedProduct.ActualWeight;
                product.TotalPrice = updatedProduct.TotalPrice;
            }

            RecalculateTotals(order);

            order.DeliveryDate = orderDto.DeliveryDate;

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

        private void RecalculateTotals(Order order)
        {
            order.TotalWeight = order.Products
                .Where(x => x.ActualWeight.HasValue)
                .Sum(x => x.ActualWeight);

            order.TotalPrice = order.Products
                .Where(x => x.TotalPrice.HasValue)
                .Sum(x => x.TotalPrice);
        }
    }
}
