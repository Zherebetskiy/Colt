﻿namespace Colt.Application.Common.Models
{
    public class OrderDto
    {
        public int? Id { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public DateTime OrderDate { get; set; }

        public int CustomerId { get; set; }

        public List<OrderProductDto> Products { get; set; }

        public double Weight { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
