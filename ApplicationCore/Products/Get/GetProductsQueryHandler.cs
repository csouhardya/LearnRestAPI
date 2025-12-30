using ApplicationCore.DataAccess;
using ApplicationCore.Models;
using MediatR;
using System.Linq.Expressions;

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
            var productsResponse = products.Products;

            if(!string.IsNullOrEmpty(request.searchTerm))
            {
                bool success = decimal.TryParse(request.searchTerm, out decimal searchAmount);
                productsResponse = [.. productsResponse.Where(_ => string.Equals(_.Name.ToLower(), request.searchTerm) ||
                                                                    _.Sku.ToLower() == request.searchTerm.ToLower() || 
                                                                    (success ? _.Amount == searchAmount : false)
                                                                    )];
            }
            if(!string.IsNullOrEmpty(request.sortBy))
            {
                productsResponse = [..this.SortHelper(productsResponse.AsQueryable(), request.sortBy, request.sortOrder)];
            }
            return Task.FromResult(productsResponse);
        }

        private IQueryable<Product> SortHelper(IQueryable<Product> products, string sortBy, string? sortOrder)
        {
            var propInfo = typeof(Product).GetProperty(sortBy, System.Reflection.BindingFlags.IgnoreCase |
                                                        System.Reflection.BindingFlags.Public |
                                                        System.Reflection.BindingFlags.Instance);
            if (propInfo == null) return products; // TODO : validate
            bool desc;
            desc = sortOrder?.ToLower() == "desc" || sortOrder?.ToLower() == "descending"; // TODO validate

            var param = Expression.Parameter(typeof(Product), "p");
            var prop = Expression.Property(param, propInfo);
            var lambda = Expression.Lambda(prop, param);

            var methodName = desc ? "OrderByDescending" : "OrderBy";

            var result = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(Product), prop.Type },
                products.Expression,
                Expression.Quote(lambda)
            );
            return products.Provider.CreateQuery<Product>(result);
        }
    }
}
