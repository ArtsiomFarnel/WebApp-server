using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Data.Entities;

namespace WebApp.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role { Name = "Manager", NormalizedName = "MANAGER" },
                new Role { Name = "Administrator", NormalizedName = "ADMINISTRATOR" },
                new Role { Name = "Client", NormalizedName = "CLIENT" });
        }
    }
}
