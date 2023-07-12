using Colt.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Colt.Infrastructure.Persistance.Configurations
{
    public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.Property(x => x.OrderId)
                .IsRequired(true);

            builder.Property(x => x.CustomerProductId)
                .IsRequired(true);

            builder.Property(x => x.TotalPrice)
                .IsRequired(true);

            builder.Property(x => x.OrderedWeight)
               .IsRequired(false);

            builder.Property(x => x.ActualWeight)
               .IsRequired(false);
        }
    }
}
