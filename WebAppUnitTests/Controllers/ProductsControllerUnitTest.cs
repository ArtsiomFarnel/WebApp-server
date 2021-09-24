using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Api.Controllers.V1;
using WebApp.Application.Abstractions;
using WebApp.Application.Abstractions.Repositories;
using WebApp.Application.Interfaces;
using WebApp.Application.Models.DataTransferObjects.Outgoing.Products;
using WebApp.Application.Models.RequestFeatures;
using WebApp.Application.Models.RequestFeatures.Product;
using WebApp.Data.Entities;
using Xunit;

namespace WebApp.WebAppUnitTests.Controllers
{
    public class ProductsControllerUnitTest
    {
        private ProductsController controller;
        private ProductParameters productParameters;
        private readonly Mock<ILoggerManager> _loggerManager;
        private readonly Mock<IRepositoryManager> _repositoryManager;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDataShaper<ProductFullInfoDto>> _dataShaper;

        private readonly PagedList<Product> productsInMemoryDatabase = new PagedList<Product>(new List<Product>
        {
            new Product { Id = 1, Name = "product1", CategoryId = 1, ProviderId = 1 },
            new Product { Id = 2, Name = "product2", CategoryId = 1, ProviderId = 1 },
            new Product { Id = 3, Name = "product3", CategoryId = 1, ProviderId = 1 }
        }, 3, 1, 1);

        private readonly Product product = new Product
        {
            Id = 1, Name = "product1", CategoryId = 1, ProviderId = 1
        };

        public ProductsControllerUnitTest()
        {
            _loggerManager = new Mock<ILoggerManager>();
            _repositoryManager = new Mock<IRepositoryManager>();
            _mapper = new Mock<IMapper>();
            _dataShaper = new Mock<IDataShaper<ProductFullInfoDto>>();

            controller = new ProductsController(_loggerManager.Object, _repositoryManager.Object, _mapper.Object, _dataShaper.Object);

            productParameters = new ProductParameters
            {
                CategoryId = 1,
                ProviderId = 1
            };
            _repositoryManager.Setup(m => m.Products.GetAllProductsAsync(It.IsAny<ProductParameters>(), It.IsAny<bool>()).Result).Returns(productsInMemoryDatabase);
            _repositoryManager.Setup(m => m.Products.GetProductByIdAsync(It.IsAny<int>(), It.IsAny<bool>()).Result).Returns(product);
        }

        [Fact]
        public async Task GetAllProductsTestMethod()
        {
            var result = await controller.GetAllProducts(productParameters);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
        }
    }
}
