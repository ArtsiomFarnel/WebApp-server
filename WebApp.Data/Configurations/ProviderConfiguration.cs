using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Data.Entities;

namespace WebApp.Data.Configurations
{
    public class ProviderConfiguration : IEntityTypeConfiguration<Provider>
    {
        public void Configure(EntityTypeBuilder<Provider> builder)
        {
            builder.HasIndex(p => p.Name).IsUnique();

            builder.HasData(
                new Provider { Id = 1, Name = "test_provider_1" },
                new Provider { Id = 2, Name = "test_provider_2" },
                new Provider { Id = 3, Name = "test_provider_3" },
                new Provider { Id = 4, Name = "test_provider_4" },
                new Provider { Id = 5, Name = "test_provider_5" });
        }
    }
}
