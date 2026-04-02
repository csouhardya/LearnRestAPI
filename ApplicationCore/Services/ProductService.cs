using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.Products.Get;
using ApplicationCore.Products.GetById;
using MediatR;

namespace ApplicationCore.Services
{
    public class ProductService : IProductService
    {
        private ISender _sender;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="sender">MediatR sender used to dispatch queries.</param>
        public ProductService(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>A task that resolves to the list of all products.</returns>
        public async Task<List<Product>> GetProductsAsync()
        {
            var query = new GetProductsQuery();
            var products = await _sender.Send(query);
            return products;
        }

        /// <summary>
        /// Retrieves products filtered and paged based on the provided parameters.
        /// </summary>
        /// <param name="searchTerm">Optional search term to filter products by name, SKU or amount.</param>
        /// <param name="sortBy">Optional property name to sort results by.</param>
        /// <param name="sortOrder">Optional sort order, e.g. "asc" or "desc".</param>
        /// <param name="page">Optional 1-based page number.</param>
        /// <param name="pageSize">Optional number of items per page.</param>
        /// <returns>A task that resolves to a paged list of products.</returns>
        public async Task<PageList<Product>> GetProductsAsync(string? searchTerm, string? sortBy, string? sortOrder, int? page, int? pageSize)
        {
            var query = new GetProductsQueryBySearchTerm(searchTerm, sortBy, sortOrder, page, pageSize);
            var products = await _sender.Send(query);
            return products;
        }

        public async Task<Product> GetProductByIdAsync(Guid guid)
        {
            var query = new GetProductQuery(guid);
            var product = await _sender.Send(query);
            return product;


        }
    }
}
