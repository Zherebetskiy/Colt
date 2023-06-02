﻿using Colt.Domain.Common;
using Colt.Domain.Enums;

namespace Colt.Domain.Entities
{
    public class Order : BaseEntity<int>
    {
        public DateTime? DeliveryDate { get; set; }

        public DateTime OrderDate { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<OrderProduct> Products { get; set; }

        public double? TotalWeight { get; set; }

        public decimal? TotalPrice { get; set; }

        public OrderStatus Status { get; set; }
    }
}
