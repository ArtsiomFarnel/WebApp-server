using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebApp.Application.Interfaces;
using WebApp.Data.Entities;
using WebApp.Infrastructure;

namespace WebApp.Application.Abstractions.Repositories
{
    public interface IUserRepository : IRepositoryBase<User>
    {
    }

    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(DatabaseContext context)
            : base(context)
        {
        }
    }
}
