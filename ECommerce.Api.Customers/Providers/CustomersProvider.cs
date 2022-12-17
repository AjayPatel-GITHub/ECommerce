using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers 
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomersDbContext dbContext;
        private readonly ILogger<CustomersProvider> logger;
        private readonly IMapper mapper;

        public CustomersProvider(CustomersDbContext dbContext , ILogger<CustomersProvider> logger, IMapper mapper )
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if(!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new Db.Customer() { Id = 1, Name = "Ajay Patel", Address = "Dahsiar" });
                dbContext.Customers.Add(new Db.Customer() { Id = 2, Name = "Vijay Patel", Address = "Dahsiar" });
                dbContext.Customers.Add(new Db.Customer() { Id = 3, Name = "Raj Patel", Address = "Dahsiar" });
                dbContext.Customers.Add(new Db.Customer() { Id = 4, Name = "Gunj Patel", Address = "Dahsiar" });
                dbContext.SaveChanges();
            }
        }



        async Task<(bool ISuccess, IEnumerable<Models.Customer> Customers, string ErrorMessage)> ICustomersProvider.GetCustomersAsync()
        {
            try
            {
                var customers = await dbContext.Customers.ToListAsync();

                if (customers != null && customers.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(customers);
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

        async Task<(bool ISuccess, Models.Customer customer, string ErrorMessage)> ICustomersProvider.GetCustomerAsync(int Id)
        {
            try
            {
                var customer = await dbContext.Customers.FirstOrDefaultAsync(p => p.Id == Id);

                if (customer != null)
                {
                    var result = mapper.Map<Db.Customer, Models.Customer>(customer);
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
