using AutoMapper;
using Colt.Application.Common.Models;
using Colt.Domain.Entities;

namespace Colt.Application.Common.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>(MemberList.Destination);
            CreateMap<OrderDto, Order>(MemberList.Destination);

            CreateMap<OrderProduct, OrderProductDto>(MemberList.Destination);
            CreateMap<OrderProductDto, OrderProduct>(MemberList.Destination);
        }
    }
}
