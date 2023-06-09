﻿namespace Colt.Application.Common.Models
{
    public class CustomerDto
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public List<CustomerProductDto> Products { get; set; } = new List<CustomerProductDto>();
    }
}
