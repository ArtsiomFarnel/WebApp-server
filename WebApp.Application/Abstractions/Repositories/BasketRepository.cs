using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Data.Entities;
using WebApp.Infrastructure;

namespace WebApp.Application.Abstractions.Repositories
{
    public interface IBasketRepository : IRepositoryBase<Basket>
    {
    }

    public class BasketRepository : RepositoryBase<Basket>, IBasketRepository
    {
        public BasketRepository(DatabaseContext context)
            : base(context)
        {
        }
    }
}
