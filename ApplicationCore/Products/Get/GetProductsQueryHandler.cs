using ApplicationCore.DataAccess;
using ApplicationCore.Models;
using MediatR;

namespace ApplicationCore.Products.Get
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<Product>>, IRequestHandler<GetProductsQueryBySearchTerm, List<Product>>
    {
        public Task<List<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            ProductsData products = new();
            return Task.FromResult(products.Products);
        }

        public Task<List<Product>> Handle(GetProductsQueryBySearchTerm request, CancellationToken cancellationToken)
        {
            ProductsData products = new();
            bool success = decimal.TryParse(request.searchTerm, out decimal searchAmount);
            var productsResponse = products.Products.Where(_ => _.Name.ToLower() == request.searchTerm.ToLower() ||
                                                                _.Sku.ToLower() == request.searchTerm.ToLower() || 
                                                                success ? _.Amount == searchAmount : false
                                                                );
            return Task.FromResult(productsResponse.ToList());
        }
    }
}
