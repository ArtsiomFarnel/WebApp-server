using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Application.Abstractions.Repositories;
using WebApp.Data.Entities;
using WebApp.Infrastructure;

namespace WebApp.WebAppUnitTests.Repositories
{
    [TestClass]
    public class ProductsRepositoryUnitTest
    {
        private readonly List<Product> productsInMemoryDatabase = new List<Product>
        {
            new Product { Id = 1, Name = "product1", CategoryId = 1, ProviderId = 1 },
            new Product { Id = 2, Name = "product2", CategoryId = 1, ProviderId = 1 },
            new Product { Id = 3, Name = "product3", CategoryId = 1, ProviderId = 1 }
        };

        [TestMethod]
        public void GetOneTestMethod()
        {

        }
    }
}
