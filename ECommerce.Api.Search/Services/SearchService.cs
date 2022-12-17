using ECommerce.Api.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService ordersService;
        private readonly IProductsService productsService;

        public SearchService(IOrdersService ordersService, IProductsService productsService)
        {
            this.ordersService = ordersService;
            this.productsService = productsService;
        }
        public async Task<(bool IsSucess, dynamic SearchResults)> SearchAsync(int CustomerId)
        {
            var orderResult = await ordersService.GetOrderAsync(CustomerId);
            var productsResult = await productsService.GetProductsAsync();

            if (orderResult.IsSuccess)
            {
                foreach (var order in orderResult.Orders)
                {
                    foreach (var item in order.Items)
                    {
                        item.ProductName = productsResult.IsSuccess ?  productsResult.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name : "Product Information is not avaliable";
                    } 
                }

                var result = new
                {
                    Orders = orderResult.Orders
                };
                return (true, result);
            }
            return (false, null);
        }
    }
}
