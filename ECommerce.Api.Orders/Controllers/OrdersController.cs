using ECommerce.Api.Orders.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersProvider ordersProvider;

        public OrdersController(IOrdersProvider ordersProvider)
        {
            this.ordersProvider = ordersProvider;
        }

        [HttpGet("{customerid}")]
        public async Task<IActionResult> GetOrdersAsync(int customerid)
        {
            var result = await ordersProvider.GetOrdersAsync(customerid);

            if (result.IsSuccess)
            {
                return Ok(result.Orders);
            }

            return NotFound();
        }
    }
}
