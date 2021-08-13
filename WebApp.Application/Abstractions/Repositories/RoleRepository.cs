using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Data.Entities;
using WebApp.Infrastructure;

namespace WebApp.Application.Abstractions.Repositories
{
    public interface IRoleRepository : IRepositoryBase<Role>
    {
    }

    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(DatabaseContext context)
            : base(context)
        {
        }
    }
}
