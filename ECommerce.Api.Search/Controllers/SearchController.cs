using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        public async Task<IActionResult> SearchAsync(SearchTerm term)
        {
            var result = await searchService.SearchAsync(term.CustomerID);

            if(result.IsSucess)
            {
                return Ok(result.SearchResults);
            }

            return NotFound();
        }
    }
}
