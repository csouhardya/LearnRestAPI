using ApplicationCore.Interfaces;
using ApplicationCore.Misc;
using ApplicationCore.Models;
using ApplicationCore.Queries.Products.Delete;
using MediatR;
using Serilog;

namespace ApplicationCore.Queries.Products.Handlers
{
    /// <summary>
    /// MediatR handler that processes <see cref="DeleteProductsQuery"/> requests to delete a product.
    /// </summary>
    public class DeleteProductsHandler(IProductsRepository productsRepository, ICachingService cachingService, ILogger logger): IRequestHandler<DeleteProductsQuery, bool>
    {
        private readonly IProductsRepository _productsRepository = productsRepository;
        private readonly ICachingService _cachingService = cachingService;
        private readonly ILogger _logger = logger;

        /// <summary>
        /// Handles the delete-product request by delegating to the repository.
        /// </summary>
        /// <param name="request">Query containing the product GUID to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True when one or more rows were deleted; otherwise false.</returns>
        public async Task<bool> Handle(DeleteProductsQuery request, CancellationToken cancellationToken)
        {
            _logger.Information($"Sending {request.guid} to repository");
            var result = await _productsRepository.DeleteAsync(request.guid);
            if (result > 0)
            {
                _logger.Information($"Deleted data {request.guid} from guid");
                var cacheData = _cachingService.GetData<List<Product>>(Constants.AllProductCacheKey);
                if (cacheData != null)
                {
                    cacheData.Remove(cacheData.Where(_ => _.Guid == request.guid).First());
                    _cachingService.ReInsertData(Constants.AllProductCacheKey, cacheData);
                }
                return true;
            }
            _logger.Error($"Data {request.guid} cannot be deleted");
            return false;
        }
    }
}
