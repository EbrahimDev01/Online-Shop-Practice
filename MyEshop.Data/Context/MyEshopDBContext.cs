using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyEshop.Data.FluentConfigs;
using MyEshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Data.Context
{
    public class MyEshopDBContext : IdentityDbContext<ApplicationUser>
    {
        public MyEshopDBContext(DbContextOptions<MyEshopDBContext> options)
            : base(options)
        {
        }


        public virtual DbSet<Cart> Carts { set; get; }
        public virtual DbSet<Tag> Tags { set; get; }
        public virtual DbSet<Product> Products { set; get; }
        public virtual DbSet<CartItem> CartItems { set; get; }
        public virtual DbSet<Category> Categories { set; get; }
        public virtual DbSet<Comment> Comments { set; get; }
        public virtual DbSet<Image> Images { set; get; }
        public virtual DbSet<UserAddress> UserAddresses { set; get; }
        public virtual DbSet<UserWallet> UserWallets { set; get; }
        public virtual DbSet<UserWalletDetails> UserWalletDetails { set; get; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new FluentApplicationUserConfig());
            builder.ApplyConfiguration(new FluentCartConfig());
            builder.ApplyConfiguration(new FluentCartItemConfig());
            builder.ApplyConfiguration(new FluentCategoryConfig());
            builder.ApplyConfiguration(new FluentCommentConfig());
            builder.ApplyConfiguration(new FluentImageConfig());
            builder.ApplyConfiguration(new FluentProductConfig());
            builder.ApplyConfiguration(new FluentTagConfig());
            builder.ApplyConfiguration(new FluentUserAddressConfig());
            builder.ApplyConfiguration(new FluentUserWalletDetailsConfig());
            builder.ApplyConfiguration(new FluentUserWalletConfig());

            base.OnModelCreating(builder);
        }
    }
}