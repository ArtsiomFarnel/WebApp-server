using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Application.Abstractions.Repositories;
using WebApp.Data.Entities;
using WebApp.Infrastructure;
using Xunit;

namespace WebApp.WebAppUnitTests.Repositories
{
    public class ProductsRepositoryUnitTest
    {
        private readonly List<Product> productsInMemoryDatabase = new List<Product>
        {
            new Product { Id = 1, Name = "product1", CategoryId = 1, ProviderId = 1 },
            new Product { Id = 2, Name = "product2", CategoryId = 1, ProviderId = 1 },
            new Product { Id = 3, Name = "product3", CategoryId = 1, ProviderId = 1 }
        };

        [Fact]
        public async Task GetOneTestMethodAsync()
        {
            int id = 1;
            var mockRep = new Mock<IProductRepository>();
            mockRep.Setup(p => p.GetProductByIdAsync(id, false).Result)
                .Returns(productsInMemoryDatabase.FirstOrDefault(p => p.Id == id));

            var product = await mockRep.Object.GetProductByIdAsync(id, false);

            Assert.NotNull(product);
            Assert.Equal(1, product.Id);
            Assert.Equal("product1", product.Name);
            //test
        }
    }
}
