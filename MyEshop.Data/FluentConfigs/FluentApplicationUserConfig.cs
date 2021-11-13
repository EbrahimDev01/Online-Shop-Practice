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
    public class FluentApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(applicationUser => applicationUser.NationalCode)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(applicationUser => applicationUser.Birthday)
                .IsRequired();

            builder.HasOne(applicationUser => applicationUser.UserImage)
                .WithOne(image => image.ApplicationUser)
                .HasForeignKey<ApplicationUser>(applicationUser => applicationUser.UserImageId);

            builder.HasMany(applicationUser => applicationUser.UserComments)
                .WithOne(comment => comment.ApplicationUser)
                .HasForeignKey(comment => comment.ApplicationUserId);

            builder.HasMany(applicationUser => applicationUser.UserCarts)
                .WithOne(cart => cart.ApplicationUser)
                .HasForeignKey(cart => cart.ApplicationUserId);

            builder.HasMany(applicationUser => applicationUser.UserAddresses)
                .WithOne(userAddress => userAddress.ApplicationUser)
                .HasForeignKey(userAddress => userAddress.ApplicationUserId);

            builder.HasOne(applicationUser => applicationUser.UserWallet)
                .WithOne(image => image.ApplicationUser)
                .HasForeignKey<ApplicationUser>(applicationUser => applicationUser.UserWalletId);

            builder.Property(applicationUser => applicationUser.UserWalletId)
                .IsRequired();
        }
    }
}
