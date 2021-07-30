using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using WebApp.Application.Interfaces;
using WebApp.Application.Models.RequestFeatures;
using WebApp.Application.Models.RequestFeatures.Product;
using WebApp.Data.Entities;
using WebApp.Infrastructure;
using System.Reflection;
using System.Net;
using Newtonsoft.Json;
using WebApp.Application.Models;

namespace WebApp.Application.Abstractions.Repositories
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<PagedList<Product>> GetAllProductsAsync(ProductParameters param, bool trackChanges);
        Task<Product> GetProductByIdAsync(int id, bool trackChanges);
    }

    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(DatabaseContext context)
            : base(context)
        {
        }

        public async Task<PagedList<Product>> GetAllProductsAsync(ProductParameters param, bool trackChanges)
        {
            var products = await GetAll(trackChanges)
                .FilterProductsByCategoryId(param.CategoryId)
                .FilterProductsByProviderId(param.ProviderId)
                .Include(p => p.Provider)
                .Include(p => p.Category)
                .SearchProduct(param.SearchTerm)
                .ExchangeCurrencyProduct(param.Currency)
                .FilterProductByCost(param.MinCost, param.MaxCost)
                .SortProduct(param.OrderBy)
                .ToListAsync();

            return PagedList<Product>.ToPagedList(products, param.PageNumber, param.PageSize);
        } 

        public async Task<Product> GetProductByIdAsync(int id, bool trackChanges) =>
            await GetByCondition(p => p.Id == id, trackChanges)
                .Include(p => p.Provider)
                .Include(p => p.Category)
                .FirstOrDefaultAsync();
    }

    public static class ProductRepositoryExtensions
    {
        public static IQueryable<Product> FilterProductByCost(this IQueryable<Product> products, float minCost, float maxCost) =>
            products.Where(p => p.Cost >= minCost && p.Cost <= maxCost);

        public static IQueryable<Product> SearchProduct(this IQueryable<Product> products, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return products;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return products.Where(p => p.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Product> SortProduct(this IQueryable<Product> products, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return products.OrderBy(e => e.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Product>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return products.OrderBy(e => e.Name);

            return products.OrderBy(orderQuery);
        }

        public static IQueryable<Product> ExchangeCurrencyProduct(this IQueryable<Product> products, string currency)
        {
            string json_result = new WebClient().DownloadString("http://api.exchangeratesapi.io/v1/latest?access_key=e294ee9b31301144bd13142b3ed636ff&format=1");
            if (json_result == null)
            {
                return products;
            }

            var deserialize_result = JsonConvert.DeserializeObject<ExchangeRatesApiPage>(json_result);

            float? coef = 1.0f;
            if (currency.Equals("RUB"))
                coef = deserialize_result.Rates.RUB;
            if (currency.Equals("BYN"))
                coef = deserialize_result.Rates.BYN;
            if (currency.Equals("USD"))
                coef = deserialize_result.Rates.USD;

            return products.Select(p => new Product 
            {
                Provider = p.Provider,
                Category = p.Category,
                CategoryId = p.CategoryId,
                ProviderId = p.ProviderId,
                Description = p.Description,
                Name = p.Name,
                Id = p.Id,
                Cost = p.Cost * (float)coef
            });
        }

        public static IQueryable<Product> FilterProductsByProviderId(this IQueryable<Product> products, int providerId)
        {
            if (providerId != 0)
                products = products.Where(p => p.ProviderId == providerId);
            return products;
        }

        public static IQueryable<Product> FilterProductsByCategoryId(this IQueryable<Product> products, int categoryId)
        {
            if (categoryId != 0)
                products = products.Where(p => p.CategoryId == categoryId);
            return products;
        }
    }
}
