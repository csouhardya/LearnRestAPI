using ApplicationCore.Interfaces;
using ApplicationCore.Misc;
using ApplicationCore.Models;
using ApplicationCore.Queries.Products.Create;
using MediatR;

namespace ApplicationCore.Queries.Products.Handlers
{
    /// <summary>
    /// MediatR handler that processes <see cref="AddProductsQuery"/> requests to add a product.
    /// </summary>
    public class AddProductsHandler(IProductsRepository productsRepository, ICachingService cachingService) : IRequestHandler<AddProductsQuery, bool>
    {
        private IProductsRepository _productsRepository = productsRepository;
        private readonly ICachingService _cachingService = cachingService;

        /// <summary>
        /// Handles the add-product request by delegating to the repository.
        /// </summary>
        /// <param name="request">Query containing the product to add.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True when one or more rows were inserted; otherwise false.</returns>
        public async Task<bool> Handle(AddProductsQuery request, CancellationToken cancellationToken)
        {
            var isAdded =  await _productsRepository.AddAsync(request.product);
            if (isAdded > 0)
            {
                var cacheData = _cachingService.GetData<List<Product>>(Constants.AllProductCacheKey);
                if (cacheData != null)
                {
                    cacheData.Add(request.product);
                    _cachingService.ReInsertData(Constants.AllProductCacheKey, cacheData);
                }
                return true;
            }
            return false;
        }
    }
}
