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
    class FluentUserWalletDetailsConfig : IEntityTypeConfiguration<UserWalletDetails>
    {
        public void Configure(EntityTypeBuilder<UserWalletDetails> builder)
        {
            builder.HasKey(userWalletDetails => userWalletDetails.UserWalletDetailsId);
        }
    }
}
