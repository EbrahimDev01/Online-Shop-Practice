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
    class FluentProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(product => product.ProductId);

            builder.Property(product => product.Title)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(product => product.Descritption)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(product => product.Explanation)
                .IsRequired();

            builder.Property(product => product.CategoryId)
                .IsRequired();

            builder.HasMany(product => product.Images)
                .WithOne(image => image.Product)
                .HasForeignKey(image => image.ProductId);

            builder.HasMany(product => product.Comments)
                .WithOne(comment => comment.Product)
                .HasForeignKey(comment => comment.ProductId);

            builder.HasMany(product => product.CartItems)
                .WithOne(cartItem => cartItem.Product)
                .HasForeignKey(cartItem => cartItem.ProductId);

            builder.HasOne(product => product.Category)
                .WithMany(category => category.Products)
                .HasForeignKey(product => product.ProductId);

            builder.HasMany(product => product.Tags)
                .WithMany(tag => tag.Products); 

        }
    }
}
