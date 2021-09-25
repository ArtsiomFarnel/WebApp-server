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
    public class ProductsControllerIntegrateUnitTest
    {
        private readonly HttpClient _client;

        public ProductsControllerIntegrateUnitTest()
        {
            var webBuilder = new WebHostBuilder()
                .UseStartup<Startup>();

            var server = new TestServer(webBuilder);

            _client = server.CreateClient();
        }

        [Fact]
        public async Task GetAllProductsTestMethodAsync()
        {
            var apiResponse = await _client.GetAsync("/v2/products/get_all_products");
            Assert.Equal(StatusCodes.Status200OK, (int) apiResponse.StatusCode);
        }
    }
}
