using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using WebApp.Application.Interfaces;
using WebApp.Application.Models.RequestFeatures;
using WebApp.Application.Models.RequestFeatures.Category;
using WebApp.Data.Entities;
using WebApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebApp.Application.Models.RequestFeatures.Provider;

namespace WebApp.Application.Abstractions.Repositories
{
    public interface IProviderRepository : IRepositoryBase<Provider>
    {
        Task<PagedList<Provider>> GetAllProvidersAsync(ProviderParameters param, bool trackChanges);
        Task<Provider> GetProviderByIdAsync(int id, bool trackChanges);
    }

    public class ProviderRepository : RepositoryBase<Provider>, IProviderRepository
    {
        public ProviderRepository(DatabaseContext context)
            : base(context)
        {
        }

        public async Task<PagedList<Provider>> GetAllProvidersAsync(ProviderParameters param, bool trackChanges)
        {
            var providers = await GetAll(trackChanges)
                .SearchProvider(param.SearchTerm)
                .SortProvider(param.OrderBy)
                .ToListAsync();

            return PagedList<Provider>.ToPagedList(providers, param.PageNumber, param.PageSize);
        }

        public async Task<Provider> GetProviderByIdAsync(int id, bool trackChanges) =>
            await GetByCondition(p => p.Id == id, trackChanges)
                .FirstOrDefaultAsync();
    }

    public static class ProviderRepositoryExtensions
    {
        public static IQueryable<Provider> SearchProvider(this IQueryable<Provider> providers, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return providers;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return providers.Where(p => p.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Provider> SortProvider(this IQueryable<Provider> providers, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return providers.OrderBy(p => p.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Provider>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return providers.OrderBy(p => p.Name);

            return providers.OrderBy(orderQuery);
        }
    }
}
