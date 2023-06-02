namespace Colt.Application.Common.Models
{
    public class OrderProductDto
    {
        public int? Id { get; set; }

        public int CustomerProductId { get; set; }

        public decimal ProductPrice { get; set; }

        public int? OrderdItemsAmount { get; set; }
        public double? OrderdItemsWeight { get; set; }

        public int? ActualItemsAmount { get; set; }
        public double? ActualItemsWeight { get; set; }
    }
}
