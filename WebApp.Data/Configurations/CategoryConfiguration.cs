using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Data.Entities;

namespace WebApp.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasIndex(c => c.Name).IsUnique();

            builder.HasData(
                new Category { Id = 1, Name = "test_category_1" },
                new Category { Id = 2, Name = "test_category_2" },
                new Category { Id = 3, Name = "test_category_3" },
                new Category { Id = 4, Name = "test_category_4" },
                new Category { Id = 5, Name = "test_category_5" });
        }
    }
}
