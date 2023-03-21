using AutoMapper;
using Colt.Application.Common.Models;
using Colt.Domain.Entities;

namespace Colt.Application.Common.Mappings
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>(MemberList.Destination);
        }
    }
}
