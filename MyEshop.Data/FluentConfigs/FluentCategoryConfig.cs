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
    public class FluentCategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(category => category.CategoryId);

            builder.Property(category => category.Title)
                .HasMaxLength(150)
                .IsRequired();

            builder.HasMany(category => category.CategoriesChildren)
                .WithOne(category => category.CategoryParent)
                .HasForeignKey(category => category.CategoryParentId);
        }
    }
}
