﻿using Colt.Domain.Common;

namespace Colt.Domain.Entities
{
    public class Order : BaseEntity<int>
    {
        public DateTime? DeliveryDate { get; set; }

        public DateTime OrderDate { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public double Weight { get; set; }

        public decimal TotalPrice { get; set; }
    }
}