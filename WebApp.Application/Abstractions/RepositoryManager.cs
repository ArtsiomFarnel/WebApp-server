using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApp.Application.Abstractions.Repositories;
using WebApp.Application.Interfaces;
using WebApp.Infrastructure;

namespace WebApp.Application.Abstractions
{
    public interface IRepositoryManager
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        ICategoryRepository Categories { get; }
        IProviderRepository Providers { get; }
        IProductRepository Products { get; }
        IBasketRepository Baskets { get; }
        Task SaveAsync();
    }

    public class RepositoryManager : IRepositoryManager
    {
        private readonly DatabaseContext _context;

        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private ICategoryRepository _categoryRepository;
        private IProviderRepository _providerRepository;
        private IProductRepository _productRepository;
        private IBasketRepository _basketRepository;

        public RepositoryManager(DatabaseContext context)
        {
            _context = context;
        }

        public IUserRepository Users
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_context);
                return _userRepository;
            }
        }

        public IRoleRepository Roles
        {
            get
            {
                if (_roleRepository == null)
                    _roleRepository = new RoleRepository(_context);
                return _roleRepository;
            }
        }

        public ICategoryRepository Categories
        {
            get
            {
                if (_categoryRepository == null)
                    _categoryRepository = new CategoryRepository(_context);
                return _categoryRepository;
            }
        }

        public IProviderRepository Providers
        {
            get
            {
                if (_providerRepository == null)
                    _providerRepository = new ProviderRepository(_context);
                return _providerRepository;
            }
        }

        public IProductRepository Products
        {
            get
            {
                if (_productRepository == null)
                    _productRepository = new ProductRepository(_context);
                return _productRepository;
            }
        }

        public IBasketRepository Baskets
        {
            get
            {
                if (_basketRepository == null)
                    _basketRepository = new BasketRepository(_context);
                return _basketRepository;
            }
        }

        public async Task SaveAsync() => 
            await _context.SaveChangesAsync();
    }
}
