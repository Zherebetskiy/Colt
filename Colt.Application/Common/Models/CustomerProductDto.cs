﻿namespace Colt.Application.Common.Models
{
    public class CustomerProductDto
    {
        public int? Id { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        public decimal Price { get; set; }
    }
}
