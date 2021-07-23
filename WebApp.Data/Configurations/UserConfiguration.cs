using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Data.Entities;

namespace WebApp.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        //
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //builder.HasData();
        }
    }
}
