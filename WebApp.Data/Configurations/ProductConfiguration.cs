using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Data.Entities;

namespace WebApp.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            //builder.HasIndex(p => new { p.Name, p.Description, p.Cost, p.CategoryId, p.ProviderId }).IsUnique();

            builder.HasData(
                new Product { Id = 1, Name = "test_product_1", Cost = 1.1f, Description = "test_description_1", CategoryId = 1, ProviderId = 1 },
                new Product { Id = 2, Name = "test_product_2", Cost = 2.1f, Description = "test_description_2", CategoryId = 1, ProviderId = 1 },
                new Product { Id = 3, Name = "test_product_3", Cost = 1.6f, Description = "test_description_3", CategoryId = 1, ProviderId = 1 },
                new Product { Id = 4, Name = "test_product_4", Cost = 16.15f, Description = "test_description_4", CategoryId = 1, ProviderId = 1 },
                new Product { Id = 5, Name = "test_product_5", Cost = 11.11f, Description = "test_description_5", CategoryId = 1, ProviderId = 1 },
                new Product { Id = 6, Name = "test_product_6", Cost = 14.17f, Description = "test_description_6", CategoryId = 2, ProviderId = 2 },
                new Product { Id = 7, Name = "test_product_7", Cost = 5.5f, Description = "test_description_7", CategoryId = 2, ProviderId = 2 },
                new Product { Id = 8, Name = "test_product_8", Cost = 71.11f, Description = "test_description_8", CategoryId = 2, ProviderId = 2 },
                new Product { Id = 9, Name = "test_product_9", Cost = 10.06f, Description = "test_description_9", CategoryId = 2, ProviderId = 2 },
                new Product { Id = 10, Name = "test_product_10", Cost = 0.01f, Description = "test_description_10", CategoryId = 3, ProviderId = 3 },
                new Product { Id = 11, Name = "test_product_11", Cost = 113.56f, Description = "test_description_11", CategoryId = 3, ProviderId = 3 },
                new Product { Id = 12, Name = "test_product_12", Cost = 4.7f, Description = "test_description_12", CategoryId = 3, ProviderId = 3 },
                new Product { Id = 13, Name = "test_product_13", Cost = 167.9f, Description = "test_description_13", CategoryId = 4, ProviderId = 4 },
                new Product { Id = 14, Name = "test_product_14", Cost = 6.66f, Description = "test_description_14", CategoryId = 4, ProviderId = 4 },
                new Product { Id = 15, Name = "test_product_15", Cost = 9.99f, Description = "test_description_15", CategoryId = 5, ProviderId = 5 });
        }
    }
}
