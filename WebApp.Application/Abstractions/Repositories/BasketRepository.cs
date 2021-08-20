using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Application.Models.RequestFeatures;
using WebApp.Application.Models.RequestFeatures.Baskets;
using WebApp.Data.Entities;
using WebApp.Infrastructure;

namespace WebApp.Application.Abstractions.Repositories
{
    public interface IBasketRepository : IRepositoryBase<Basket>
    {
        Task<PagedList<Basket>> GetAllBasketItemsAsync(BasketParameters param, bool trackChanges, string userId);
        Task<Basket> GetBasketItemByIdAsync(int id, bool trackChanges, string userId);
        Task<Basket> GetBasketItemByProductIdAsync(int id, bool trackChanges, string userId);
    }

    public class BasketRepository : RepositoryBase<Basket>, IBasketRepository
    {
        public BasketRepository(DatabaseContext context)
            : base(context)
        {
        }

        public async Task<PagedList<Basket>> GetAllBasketItemsAsync(BasketParameters param, bool trackChanges, string userId)
        {
            var basket = await GetAll(trackChanges)
                .Where(b => b.UserId.Equals(userId))
                .Include(b => b.Product)
                .ToListAsync();

            return PagedList<Basket>.ToPagedList(basket, param.PageNumber, param.PageSize);
        }

        public async Task<Basket> GetBasketItemByIdAsync(int id, bool trackChanges, string userId) =>
            await GetByCondition(p => p.Id == id, trackChanges)
                .Where(b => b.UserId.Equals(userId))
                .Include(b => b.Product)
                .FirstOrDefaultAsync();

        public async Task<Basket> GetBasketItemByProductIdAsync(int id, bool trackChanges, string userId) =>
            await GetByCondition(p => p.ProductId == id, trackChanges)
                .Where(b => b.UserId.Equals(userId))
                .Include(b => b.Product)
                .FirstOrDefaultAsync();
    }

    public static class BasketRepositoryExtensions
    {

    }
}
