using ApplicationCore.Interfaces;
using ApplicationCore.Misc;
using ApplicationCore.Models;
using ApplicationCore.Queries.Products.Update;
using MediatR;

namespace ApplicationCore.Queries.Products.Handlers
{
    /// <summary>
    /// MediatR handler that processes <see cref="UpdateQueryAsync"/> requests to update a product.
    /// </summary>
    public class UpdateProductsHandler(IProductsRepository productsRepository , ICachingService cachingService) : IRequestHandler<UpdateQueryAsync, bool>
    {
        private readonly IProductsRepository _productsRepository = productsRepository;
        private readonly ICachingService _cachingService = cachingService;

        /// <summary>
        /// Handles the update-product request by delegating to the repository.
        /// </summary>
        /// <param name="request">Query containing the product to update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True when one or more rows were updated; otherwise false.</returns>
        public async Task<bool> Handle(UpdateQueryAsync request, CancellationToken cancellationToken)
        {
            var result = await _productsRepository.UpdateAsync(request.product);
            if (result > 0)
            {
                var cacheData = _cachingService.GetData<List<Product>>(Constants.AllProductCacheKey);
                if(cacheData != null)
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
