using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Interfaces
{
    public interface IProductService
    {
        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>A task that resolves to a list of all products.</returns>
        Task<List<Product>> GetProductsAsync();

        /// <summary>
        /// Retrieves products filtered, sorted and paginated based on provided parameters.
        /// </summary>
        /// <param name="searchTerm">Optional search term to filter products by name, SKU or amount.</param>
        /// <param name="sortBy">Optional property name to sort results by.</param>
        /// <param name="sortOrder">Optional sort order, e.g. "asc" or "desc".</param>
        /// <param name="page">Optional 1-based page number.</param>
        /// <param name="pageSize">Optional number of items per page.</param>
        /// <returns>A task that resolves to a paged list of products.</returns>
        Task<PageList<Product>> GetProductsAsync(string? searchTerm, string? sortBy, string? sortOrder, int? page, int? pageSize);

        Task<Product> GetProductByIdAsync(Guid guid);
    }
}
