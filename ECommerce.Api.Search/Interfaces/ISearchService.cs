using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Interfaces
{
    public interface ISearchService
    {
        Task<(bool IsSucess, dynamic SearchResults)> SearchAsync(int CustomerId);
    }
}
