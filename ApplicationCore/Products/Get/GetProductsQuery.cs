using ApplicationCore.Models;
using MediatR;

namespace ApplicationCore.Products.Get
{
    /// <summary>
    /// Request to retrieve all products.
    /// </summary>
    public record GetProductsQuery : IRequest<List<Product>>;

    /// <summary>
    /// Request to retrieve products filtered by search term with optional sorting and pagination.
    /// </summary>
    /// <param name="searchTerm">Search string to filter by name/SKU/amount.</param>
    /// <param name="sortBy">Property name to sort by.</param>
    /// <param name="sortOrder">Sort direction, "asc" or "desc".</param>
    /// <param name="page">1-based page number.</param>
    /// <param name="pageSize">Number of items per page.</param>
    public record GetProductsQueryBySearchTerm(string? searchTerm, string? sortBy, string? sortOrder, int? page, int? pageSize): IRequest<PageList<Product>>;
}
 