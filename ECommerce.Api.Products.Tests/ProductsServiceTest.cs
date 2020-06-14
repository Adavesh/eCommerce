using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Api.Products.Tests
{
    public class ProductsServiceTest
    {
        [Fact]
        public async Task GetProducts_ReturnsAllProduts()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder();
            dbContextOptionsBuilder.UseInMemoryDatabase("GetProducts_ReturnsAllProduts");
            var dbContext = new Db.ProductsDbContext(dbContextOptionsBuilder.Options);
            CreateData(dbContext);

            var mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<Db.Product, Models.Product>();
            }).CreateMapper();


            var productsProvider = new ProductsProvider(dbContext, null, mapper);
            var productsResult = await productsProvider.GetProductsAsync();

            Assert.True(productsResult.IsSuccess);
            Assert.Equal(10, productsResult.Products.Count());
            Assert.Null(productsResult.ErrorMessage);
        }

        [Fact]
        public async Task GetProduct_ReturnsProduct_ValidId()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder();
            dbContextOptionsBuilder.UseInMemoryDatabase("GetProduct_ReturnsProduct_ValidId");
            var dbContext = new Db.ProductsDbContext(dbContextOptionsBuilder.Options);
            CreateData(dbContext);

            var mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<Db.Product, Models.Product>();
            }).CreateMapper();


            var productsProvider = new ProductsProvider(dbContext, null, mapper);
            var productResult = await productsProvider.GetProductAsync(5);

            Assert.True(productResult.IsSuccess);
            Assert.NotNull(productResult.Product);
            Assert.Equal(5, productResult.Product.Id);
            Assert.Null(productResult.ErrorMessage);
        }

        [Fact]
        public async Task GetProduct_ReturnsProduct_InvalidId()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder();
            dbContextOptionsBuilder.UseInMemoryDatabase("GetProduct_ReturnsProduct_InvalidId");
            var dbContext = new Db.ProductsDbContext(dbContextOptionsBuilder.Options);
            CreateData(dbContext);

            var mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<Db.Product, Models.Product>();
            }).CreateMapper();


            var productsProvider = new ProductsProvider(dbContext, null, mapper);
            var productResult = await productsProvider.GetProductAsync(-1);

            Assert.False(productResult.IsSuccess);
            Assert.Null(productResult.Product);
            Assert.NotNull(productResult.ErrorMessage);
        }

        private void CreateData(Db.ProductsDbContext dbContext)
        {
            for (int i = 1; i <= 10; i++)
            {
                var product = new Db.Product(i, $"Product-{i}", i * 3.14M, 100);
                dbContext.Products.Add(product);
            }

            dbContext.SaveChanges();
        }
    }
}
