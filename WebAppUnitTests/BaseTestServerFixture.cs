using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using WebApp.Api;

namespace WebApp.WebAppUnitTests
{
    public class BaseTestServerFixture
    {
        public TestServer TestServer { get; }
        public HttpClient Client { get; }

        public BaseTestServerFixture()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var webBuilder = new WebHostBuilder()
                .UseConfiguration(builder.Build())
                .UseStartup<Startup>();

            TestServer = new TestServer(webBuilder);
            Client = TestServer.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
            TestServer.Dispose();
        }
    }
}
