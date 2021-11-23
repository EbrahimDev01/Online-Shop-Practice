
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyEshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Data.FluentConfigs
{
    public class FluentCartConfig : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(cart => cart.CartId);

            builder.HasMany(cart => cart.CartItems)
                .WithOne(cartItem => cartItem.Cart)
                .HasForeignKey(cartItem => cartItem.CartId);

            builder.HasMany(cart => cart.UserWalletDetails)
                .WithOne(userWalletDetails => userWalletDetails.Cart)
                .HasForeignKey(userWalletDetails => userWalletDetails.CartId);
        }
    }
}
