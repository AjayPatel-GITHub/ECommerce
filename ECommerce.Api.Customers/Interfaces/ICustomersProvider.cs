using ECommerce.Api.Customers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Interfaces
{
    public interface ICustomersProvider
    {
        Task<(bool ISuccess, IEnumerable<Customer> Customers, string ErrorMessage)> GetCustomersAsync();

        Task<(bool ISuccess, Customer customer, string ErrorMessage)> GetCustomerAsync(int Id);

    }
}
