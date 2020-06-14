using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Api.Products.Providers
{
    public class ProductsProvider : IProductsProvider
    {
        private readonly ProductsDbContext dbContext;
        private readonly ILogger<ProductsProvider> logger;
        private readonly IMapper mapper;

        public ProductsProvider(ProductsDbContext dbContext, ILogger<ProductsProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Products.Any())
            {
                dbContext.Products.AddRange(new[]
                {
                    new Db.Product(1, "Keyboard", 20, 100),
                    new Db.Product(2, "Mouse", 5, 100),
                    new Db.Product(3, "Monitor", 150, 200),
                    new Db.Product(4, "CPU", 200, 100),
                    new Db.Product(5, "Motherboard", 190, 200),
                });

                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, Models.Product Product, string ErrorMessage)> GetProductAsync(int productId)
        {
            try
            {
                var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (product != null)
                {
                    var result = mapper.Map<Models.Product>(product);
                    return (true, result, null);
                }

                return (false, null, "Not Found");
            }
            catch (Exception e)
            {
                logger?.LogError(e.Message, e);
                return (false, null, e.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Product> Products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                var products = await dbContext.Products.ToListAsync();
                if (products != null && products.Count > 0)
                {
                    var results = mapper.Map<IEnumerable<Models.Product>>(products);
                    return (true, results, null);
                }

                return (false, null, "Not Found");
            }
            catch (Exception e)
            {
                logger?.LogError(e.Message, e);
                return (false, null, e.Message);
            }
        }
    }
}
