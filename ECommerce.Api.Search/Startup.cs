using System;
using System.Net.Http;
using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;

namespace ECommerce.Api.Search
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<ICustomerService, CustomersService>();

            services.AddHttpClient("OrdersService", config =>
            {
                config.BaseAddress = new Uri(Configuration["Services:Orders"]);
            });

            services.AddHttpClient("ProductsService", config =>
            {
                config.BaseAddress = new Uri(Configuration["Services:Products"]);
                config.Timeout = TimeSpan.FromMilliseconds(500);
            }).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(500)));

            services.AddHttpClient("CustomersService", httpClient =>
            {
                httpClient.BaseAddress = new Uri(Configuration["Services:Customers"]);
                httpClient.Timeout = TimeSpan.FromMilliseconds(500);
            }).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(500)));

            services.AddControllers();
        }

        private IAsyncPolicy<HttpResponseMessage> pollyhandle(PolicyBuilder<HttpResponseMessage> p)
        {
            var retryPolicy = p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(500));
            return retryPolicy;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
