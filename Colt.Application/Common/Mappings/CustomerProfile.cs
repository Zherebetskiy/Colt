using AutoMapper;
using Colt.Application.Commands.Customers;
using Colt.Application.Common.Models;
using Colt.Domain.Entities;

namespace Colt.Application.Common.Mappings
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>(MemberList.Destination);

            CreateMap<CustomerProductDto, CustomerProduct>(MemberList.Destination);

            CreateMap<CreateCustomerCommand, Customer>()
                .ForMember(
                dest => dest.Products,
                opt => opt.MapFrom(src => src
                    .Products
                    .Select(x => new CustomerProduct { ProductId = x.Key, Price = x.Value })
                    .ToList()));
        }
    }
}
