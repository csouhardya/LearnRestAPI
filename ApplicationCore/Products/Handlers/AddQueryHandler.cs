using ApplicationCore.Interfaces;
using ApplicationCore.Products.Create;
using MediatR;

namespace ApplicationCore.Products.Handlers
{
    /// <summary>
    /// MediatR handler that processes <see cref="AddProductsQuery"/> requests to add a product.
    /// </summary>
    public class AddQueryHandler(IProductsRepository productsRepository) : IRequestHandler<AddProductsQuery, bool>
    {
        private IProductsRepository _productsRepository = productsRepository;

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
                return true;
            return false;
        }
    }
}
