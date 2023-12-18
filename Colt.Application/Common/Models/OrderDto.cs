using Colt.Domain.Enums;

namespace Colt.Application.Common.Models
{
    public class OrderDto
    {
        public int? Id { get; set; }

        public DateTime DeliveryDate { get; set; }

        public DateTime OrderDate { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public OrderStatus Status { get; set; }

        public List<OrderProductDto> Products { get; set; } = new List<OrderProductDto>();

        public double? TotalWeight { get; set; }

        public decimal? TotalPrice { get; set; }

        public bool CanDeliver => Status != OrderStatus.Delivered;
    }
}
