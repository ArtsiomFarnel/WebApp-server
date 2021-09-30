using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApp.Api;
using Xunit;

namespace WebApp.WebAppUnitTests.Controllers
{
    public class ProductsControllerIntegrateUnitTest : IClassFixture<BaseTestServerFixture>
    {
        private readonly BaseTestServerFixture _fixture;

        public ProductsControllerIntegrateUnitTest(BaseTestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllProductsTestMethodAsync()
        {
            var apiResponse = await _fixture.Client.GetAsync("v2/products/get_all_products");
            //apiResponse.EnsureSuccessStatusCode();
            var s = await apiResponse.Content.ReadAsStringAsync();
            Assert.Equal(StatusCodes.Status200OK, (int) apiResponse.StatusCode);
        }
    }
}
