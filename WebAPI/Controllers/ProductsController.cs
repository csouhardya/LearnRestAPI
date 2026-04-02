using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductService _productsService;
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="productService">Service used to query products.</param>
        public ProductsController(IProductService productService)
        {
            _productsService = productService;
        }

        /// <summary>
        /// Returns the full list of products.
        /// </summary>
        /// <returns>HTTP 200 with a list of products.</returns>
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productsService.GetProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// Returns products matching the provided search, sort and pagination parameters.
        /// </summary>
        /// <param name="searchTerm">Optional search text to filter by name, SKU or amount.</param>
        /// <param name="sortBy">Optional property name to sort by.</param>
        /// <param name="sortOrder">Optional sort direction ("asc" or "desc").</param>
        /// <param name="page">Optional page number (1-based).</param>
        /// <param name="pageSize">Optional page size.</param>
        /// <returns>HTTP 200 with a paged list of products.</returns>
        [HttpGet]
        [Route("SearchTerm")]
        public async Task<IActionResult> GetProductsWithSearchTerm(string? searchTerm, string? sortBy, string? sortOrder, int? page, int? pageSize)
        {
            //TODO validate if page != null then pageSize is required and minimum number should be 1
            var products = await _productsService.GetProductsAsync(searchTerm, sortBy, sortOrder, page, pageSize);
            return Ok(products);
        }

        /// <summary>
        /// Returns a single product by GUID. (Not implemented)
        /// </summary>
        /// <param name="guid">Product GUID as string.</param>
        /// <returns>HTTP 200 with the product or placeholder response.</returns>
        [HttpGet]
        [Route("Guid")]
        public async Task<IActionResult> GetProductById(Guid guid)
        {
            var products = await _productsService.GetProductByIdAsync(guid);
            return Ok(products);
        }
    }
}
