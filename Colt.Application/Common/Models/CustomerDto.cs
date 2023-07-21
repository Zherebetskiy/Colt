namespace Colt.Application.Common.Models
{
    public class CustomerDto
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public List<CustomerProductDto> Products { get; set; } = new List<CustomerProductDto>();

        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
    }
}
