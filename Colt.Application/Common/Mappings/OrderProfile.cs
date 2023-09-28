using AutoMapper;
using Colt.Application.Common.Models;
using Colt.Domain.Entities;

namespace Colt.Application.Common.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>(MemberList.Destination)
                .ForMember(dst => dst.CustomerName, opt => opt.MapFrom(src => src.Customer.Name));
            CreateMap<OrderDto, Order>(MemberList.Destination);

            CreateMap<OrderProduct, OrderProductDto>(MemberList.Destination)
                .ForMember(dst => dst.ProductName, opt => opt.MapFrom(src => src.CustomerProduct.Product.Name))
                .ForMember(dst => dst.ProductPrice, opt => opt.MapFrom(src => src.CustomerProduct.Price));
            CreateMap<OrderProductDto, OrderProduct>(MemberList.Destination);
        }
    }
}
