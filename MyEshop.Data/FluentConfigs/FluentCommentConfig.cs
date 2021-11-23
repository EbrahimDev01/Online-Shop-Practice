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
    public class FluentCommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(comment => comment.CommentId);

            builder.Property(comment => comment.Title)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(comment => comment.Title)
                .HasMaxLength(500)
                .IsRequired();
        }
    }
}
