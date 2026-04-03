using ApplicationCore.Interfaces;
using ApplicationCore.Products.Update;
using MediatR;

namespace ApplicationCore.Products.Handlers
{
    /// <summary>
    /// MediatR handler that processes <see cref="UpdateQueryAsync"/> requests to update a product.
    /// </summary>
    public class UpdateQueryHandler(IProductsRepository productsRepository) : IRequestHandler<UpdateQueryAsync, bool>
    {
        private readonly IProductsRepository _productsRepository = productsRepository;

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
                return true;
            return false;
        }
    }
}
