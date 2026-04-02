using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.Products.GetById;
using MediatR;
using System.Linq.Expressions;

namespace ApplicationCore.Products.Get
{
    public class GetQueryHandlers : IRequestHandler<GetProductsQuery, List<Product>>, IRequestHandler<GetProductsQueryBySearchTerm, PageList<Product>>, IRequestHandler<GetProductQuery, Product>
    {
        private IProductsRepository _productsRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetQueryHandlers"/> class.
        /// </summary>
        /// <param name="productsRepo">Repository used to access product data.</param>
        public GetQueryHandlers(IProductsRepository productsRepo)
        {
            _productsRepo = productsRepo;
        }


        /// <summary>
        /// Handles retrieval of all products.
        /// </summary>
        public async Task<List<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productsRepo.GetAllAsync();
            return products;
        }

        /// <summary>
        /// Handles retrieval of products filtered, sorted and paginated based on request parameters.
        /// </summary>
        public async Task<PageList<Product>> Handle(GetProductsQueryBySearchTerm request, CancellationToken cancellationToken)
        {
            var products = await _productsRepo.GetAllAsync();

            if(!string.IsNullOrEmpty(request.searchTerm))
            {
                bool success = decimal.TryParse(request.searchTerm, out decimal searchAmount);
                products = [.. products.Where(_ => string.Equals(_.Name.ToLower(), request.searchTerm) ||
                                                                    _.Sku.ToLower() == request.searchTerm.ToLower() || 
                                                                    (success ? _.Amount == searchAmount : false)
                                                                    )];
            }
            if(!string.IsNullOrEmpty(request.sortBy))
            {
                products = [..this.SortHelper(products.AsQueryable(), request.sortBy, request.sortOrder)];
            }

            int page = request.page.HasValue ? request.page.Value : 0;
            int pageSize = request.pageSize.HasValue ? request.pageSize.Value : products.Count();

            var response = PaginationHandler<Product>.Paginate(products.AsQueryable(), page, pageSize);
            return response;
        }

        /// <summary>
        /// Handles retrieval of a single product by GUID.
        /// </summary>
        public async Task<Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _productsRepo.GetAsync(request.guid);
            return product;
        }

        /// <summary>
        /// Applies ordering to the provided queryable sequence based on the specified property and direction.
        /// </summary>
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
