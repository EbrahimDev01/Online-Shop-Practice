using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyEshop.Domain.Models;

namespace MyEshop.Data.FluentConfigs
{
    public class FluentUserAddressConfig : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.HasKey(userAddress => userAddress.UserAddressId);

            builder.Property(userAddress => userAddress.State)
                .IsRequired();

            builder.Property(userAddress => userAddress.PostalCard)
                .IsRequired();

            builder.Property(userAddress => userAddress.City)
                .IsRequired();

            builder.Property(userAddress => userAddress.PostalAddress)
                .IsRequired();

            builder.Property(userAddress => userAddress.Plaque)
                .IsRequired();

            builder.Property(userAddress => userAddress.UnitNumber)
                .IsRequired();
        }
    }
}
