using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Data.Entities
{
    public class Role : IdentityRole
    {
        public IEnumerable<User> Users { get; set; }
    }
}
