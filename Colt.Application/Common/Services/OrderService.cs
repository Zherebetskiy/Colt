using AutoMapper;
using Colt.Application.Common.Models;
using Colt.Application.Interfaces;
using Colt.Domain.Common;
using Colt.Domain.Entities;
using Colt.Domain.Enums;
using Colt.Domain.Repositories;
using MediatR;

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
            _customerRepository= customerRepository;
            _mapper = mapper;
        }

        public async Task<OrderDto> CreateAsync(OrderDto orderDto, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(orderDto);

            var customer = await _customerRepository.GetWithProductsAsync(order.CustomerId, cancellationToken);

            double? totalWeight = null;
            decimal? totalPrice = null;

            foreach (var product in order.Products)
            {
                var customerProduct = customer.Products.FirstOrDefault(x => x.Id == product.CustomerProductId);

                if (customerProduct == null)
                {
                    throw new Exception($"CustomerProduct with id: {product.CustomerProductId} not found");
                }

                //product.ProductPrice = customerProduct.Price;

                //if (product.ActualItemsWeight.HasValue)
                //{
                //    order.Status = OrderStatus.Calculated;

                //    product.OrderProductPrice = (decimal)product.ActualItemsWeight.Value * product.ProductPrice;

                //    totalWeight += product.ActualItemsWeight.Value;
                //    totalPrice += product.OrderProductPrice;
                //}
            }

            order.TotalWeight = totalWeight;
            order.TotalPrice = totalPrice;

            await _orderRepository.AddAsync(order, cancellationToken);

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> UpdateAsync(OrderDto orderDto, CancellationToken cancellationToken)
        {
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

            //TODO continue from here


            await _orderRepository.AddAsync(order, cancellationToken);

            return _mapper.Map<OrderDto>(order);
        }
    }
}
