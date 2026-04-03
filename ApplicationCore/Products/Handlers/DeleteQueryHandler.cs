using ApplicationCore.Interfaces;
using ApplicationCore.Products.Delete;
using MediatR;

namespace ApplicationCore.Products.Handlers
{
    /// <summary>
    /// MediatR handler that processes <see cref="DeleteProductsQuery"/> requests to delete a product.
    /// </summary>
    public class DeleteQueryHandler(IProductsRepository productsRepository): IRequestHandler<DeleteProductsQuery, bool>
    {
        private readonly IProductsRepository _productsRepository = productsRepository;

        /// <summary>
        /// Handles the delete-product request by delegating to the repository.
        /// </summary>
        /// <param name="request">Query containing the product GUID to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True when one or more rows were deleted; otherwise false.</returns>
        public async Task<bool> Handle(DeleteProductsQuery request, CancellationToken cancellationToken)
        {
            var result = await _productsRepository.DeleteAsync(request.guid);
            if (result > 0)
                return true;
            return false;
        }
    }
}
