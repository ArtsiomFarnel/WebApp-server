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

namespace WebApp.Application.Abstractions.Repositories
{
    public interface ICategoryRepository : IRepositoryBase<Category>
    {
        Task<PagedList<Category>> GetAllCategoriesAsync(CategoryParameters param, bool trackChanges);
        Task<Category> GetCategoryByIdAsync(int id, bool trackChanges);
    }

    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(DatabaseContext context)
            : base(context)
        {
        }

        public async Task<PagedList<Category>> GetAllCategoriesAsync(CategoryParameters param, bool trackChanges)
        {
            var categories = await GetAll(trackChanges)
                .Include(c => c.Products)
                .ThenInclude(p => p.Provider)
                .SearchCategory(param.SearchTerm)
                .SortCategory(param.OrderBy)
                .ToListAsync();

            return PagedList<Category>.ToPagedList(categories, param.PageNumber, param.PageSize);
        }

        public async Task<Category> GetCategoryByIdAsync(int id, bool trackChanges) =>
            await GetByCondition(p => p.Id == id, trackChanges)
                .FirstOrDefaultAsync();
    }

    public static class CategoryRepositoryExtensions
    {
        public static IQueryable<Category> SearchCategory(this IQueryable<Category> categories, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return categories;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return categories.Where(с => с.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Category> SortCategory(this IQueryable<Category> categories, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return categories.OrderBy(с => с.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Category>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return categories.OrderBy(с => с.Name);

            return categories.OrderBy(orderQuery);
        }
    }
}
