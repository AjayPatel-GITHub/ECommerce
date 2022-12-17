using AutoMapper;
using ECommerce.API.Products.Db;
using ECommerce.API.Products.Profiles;
using ECommerce.API.Products.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.API.Products.Test
{
    public class ProductsServiceTest
    {
        [Fact]
        public async Task GetProductsReturnsAllProducts()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsAllProducts))
                .Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var product = await productsProvider.GetProductsAsync();
            Assert.True(product.IsSucess);
            Assert.True(product.Products.Any());
            Assert.Null(product.ErrorMessage);
        }

        [Fact]
        public async Task GetProductsReturnsValidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsValidId))
                .Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var product = await productsProvider.GetProductAsync(1);
            Assert.True(product.IsSucess);
            Assert.NotNull(product.product);
            Assert.True(product.product.Id == 1);
            Assert.Null(product.ErrorMessage);
        }

        [Fact]
        public async Task GetProductsReturnsInValidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsInValidId))
                .Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var product = await productsProvider.GetProductAsync(-1);
            Assert.False(product.IsSucess);
            Assert.Null(product.product);
            Assert.NotNull(product.ErrorMessage);
        }

        private void CreateProducts(ProductsDbContext dbContext)
        {
            for (int i = 1; i <= 10; i++)
            {
                dbContext.Products.Add(new Product()
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString(),
                    Inventory = i + 10,
                    Price = (decimal)(i * 3.14)
                });
            }
            dbContext.SaveChanges();
        }
    }
}
