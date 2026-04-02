using ApplicationCore.Models;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// Repository contract for product persistence operations.
    /// </summary>
    public interface IProductsRepository
    {
        /// <summary>
        /// Inserts a product asynchronously.
        /// </summary>
        /// <param name="product">Product to add.</param>
        void AddAsync(Product product);

        /// <summary>
        /// Retrieves all products asynchronously.
        /// </summary>
        /// <returns>List of products.</returns>
        Task<List<Product>> GetAllAsync();

        /// <summary>
        /// Retrieves a single product by its GUID asynchronously.
        /// </summary>
        /// <param name="guid">Product GUID identifier.</param>
        /// <returns>The product if found; otherwise null.</returns>
        Task<Product> GetAsync(Guid guid);
    }
}
