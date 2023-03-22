using AutoMapper;
using Colt.Application.Common.Models;
using Colt.Domain.Entities;

namespace Colt.Application.Common.Mappings
{
    internal class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>(MemberList.Destination);
        }
    }
}
