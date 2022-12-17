using AutoMapper;
using ECommerce.API.Products.Db;
using ECommerce.API.Products.Interfaces;
using ECommerce.API.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.API.Products.Providers
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
           if(!dbContext.Products.Any())
            {
                dbContext.Products.Add(new Db.Product() { Id = 1, Name = "Laptop", Price = 20, Inventory = 10 });
                dbContext.Products.Add(new Db.Product() { Id = 2, Name = "Mouse", Price = 10, Inventory = 10 });
                dbContext.Products.Add(new Db.Product() { Id = 3, Name = "Monitor", Price = 100, Inventory = 10 });
                dbContext.Products.Add(new Db.Product() { Id = 4, Name = "CPU", Price = 200, Inventory = 10 });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSucess, IEnumerable<Models.Product> Products, string ErrorMessage)> GetProductsAsync()
        {

            try
            {
                var products = await dbContext.Products.ToListAsync();

                if(products != null && products.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Product>, IEnumerable<Models.Product>>(products);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
                throw;
            }
        }

        public async Task<(bool IsSucess, Models.Product product, string ErrorMessage)> GetProductAsync(int id)
        {
            try
            {
                var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

                if (product != null)
                {
                    var result = mapper.Map<Db.Product, Models.Product>(product);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
                throw;
            }
        }
    }
}
