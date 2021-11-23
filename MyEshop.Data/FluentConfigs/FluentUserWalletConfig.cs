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
    public class FluentUserWalletConfig : IEntityTypeConfiguration<UserWallet>
    {
        public void Configure(EntityTypeBuilder<UserWallet> builder)
        {
            builder.HasKey(userWallet => userWallet.UserWalletId);

            builder.HasMany(userWallet => userWallet.UserWalletDetails)
               .WithOne(useWalletDetails => useWalletDetails.UserWallet)
               .HasForeignKey(userWalletDetails => userWalletDetails.UserWalletId);
        }
    }
}
