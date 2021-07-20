using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApp.Api.Extensions;

namespace WebApp.Api
{//барбарики
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        //мы белорусы мирные люди
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureLoggerService();

            services.ConfigureIISIntegration();
            services.ConfigureControllers();
            services.ConfigureCors();
            services.ConfigureVersioning();
            services.ConfigureModelState();

            services.ConfigureMapper();
            services.ConfigureActionFilters();
            services.ConfigureDataShaper();

            services.ConfigureSqlContext(Configuration);
            services.ConfigureRepositoryManager();
            
            services.ConfigureIdentity();
            services.ConfigureJWT(Configuration);
            services.ConfigureAuthenticationManager();
            
            services.ConfigureSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // routing
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //сэрдцам адданы€ роднай з€мли
            // swagger
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                s.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
            });
        }
    }
}
