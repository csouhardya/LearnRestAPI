using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.Products.Get;
using MediatR;

namespace ApplicationCore.Services
{
    public class ProductService : IProductService
    {
        private ISender _sender;
        public ProductService(ISender sender)
        {
            _sender = sender;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var query = new GetProductsQuery();
            var products = await _sender.Send(query);
            return products;
        }

        public async Task<PageList<Product>> GetProductsAsync(string? searchTerm, string? sortBy, string? sortOrder, int? page, int? pageSize)
        {
            var query = new GetProductsQueryBySearchTerm(searchTerm, sortBy, sortOrder, page, pageSize);
            var products = await _sender.Send(query);
            return products;
        }
    }
}
