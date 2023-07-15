namespace Colt.Application.Common.Models
{
    public class OrderProductDto
    {
        public int? Id { get; set; }

        public int CustomerProductId { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public double? OrderedWeight { get; set; }

        public double? ActualWeight { get; set; }

        public decimal? TotalPrice { get; set; }
    }
}
