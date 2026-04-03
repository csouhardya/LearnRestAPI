using ApplicationCore.Models;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// Repository contract for product persistence operations.
    /// </summary>
    public interface IProductsRepository
    {
        /// <summary>
        /// Inserts a product asynchronously and returns number of rows affected.
        /// </summary>
        /// <param name="product">Product to add.</param>
        /// <returns>Number of rows inserted.</returns>
        Task<int> AddAsync(Product product);

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

        /// <summary>
        /// Updates the specified product asynchronously and returns number of rows affected.
        /// </summary>
        /// <param name="product">Product to update.</param>
        /// <returns>Number of rows updated.</returns>
        Task<int> UpdateAsync(Product product);

        /// <summary>
        /// Deletes the product with the specified GUID asynchronously and returns number of rows affected.
        /// </summary>
        /// <param name="guid">GUID of the product to delete.</param>
        /// <returns>Number of rows deleted.</returns>
        Task<int> DeleteAsync(Guid guid);
    }
}
