using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync();
        Task<PageList<Product>> GetProductsAsync(string? searchTerm, string? sortBy, string? sortOrder, int? page, int? pageSize);
    }
}
