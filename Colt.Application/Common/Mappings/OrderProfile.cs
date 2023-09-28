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

            CreateMap<OrderProduct, OrderProductDto>(MemberList.Destination);
            CreateMap<OrderProductDto, OrderProduct>(MemberList.Destination);
        }
    }
}
