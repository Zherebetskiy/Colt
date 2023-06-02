using Colt.Domain.Common;

namespace Colt.Domain.Entities
{ 
    public class OrderProduct : BaseEntity<int>
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int CustomerProductId { get; set; }
        public CustomerProduct CustomerProduct { get; set; }

        public decimal ProductPrice { get; set; }

        public int? OrderdItemsAmount { get; set; }
        public double? OrderdItemsWeight { get; set; }

        public int? ActualItemsAmount { get; set; }
        public double? ActualItemsWeight { get; set; }
    }
}
