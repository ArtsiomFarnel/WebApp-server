using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApp.Application.Abstractions.Repositories;

namespace WebApp.Application.Abstractions
{
    public interface IRepositoryManager
    {
        IUserRepository Users { get; }
        ICategoryRepository Categories { get; }
        IProviderRepository Providers { get; }
        IProductRepository Products { get; }
        Task SaveAsync();
    }
}
