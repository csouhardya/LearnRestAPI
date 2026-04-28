using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private ILogger _logger;
        private IProductService _productsService;
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="productService">Service used to query products.</param>
        public ProductsController(IProductService productService, ILogger logger)
        {
            _productsService = productService;
            _logger = logger;
        }

        /// <summary>
        /// Returns the full list of products.
        /// </summary>
        /// <returns>HTTP 200 with a list of products.</returns>
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            _logger.Information("Fetching all products");
            var products = await _productsService.GetProductsAsync();
            _logger.Information("All products fetched successfully");
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
            _logger.Information("Getting product with search term.");
            var products = await _productsService.GetProductsAsync(searchTerm, sortBy, sortOrder, page, pageSize);

            _logger.Information("returned products successfully");
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
            // TODO :  validate
            _logger.Information($"Fetching product with guid: {guid}");
            var products = await _productsService.GetProductByIdAsync(guid);
            _logger.Information($"{guid} product fetched successfully");
            return Ok(products);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddProduct(Product product)
        {
            // TODO : validate
            _logger.Information($"Adding new product {product.Name}");
            var isAdded = await _productsService.AddProductAsync(product);
            if (isAdded)
            {
                _logger.Information($"New product added successfully");
                return Created();
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            // TODO :  validate
            _logger.Information($"Updating product {product.Guid}");
            var isUpdated = await _productsService.UpdateProductAsync(product);
            if (isUpdated)
            {
                _logger.Information($"Successfully updated product {product.Guid}");
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("")]
        public async Task<IActionResult> DeleteProduct(Guid guid)
        {
            //TODO : validate
            _logger.Information($"Deleting product {guid}");
            var isDeleted = await _productsService.DeleteProductAsync(guid);
            if (isDeleted)
            {
                _logger.Information($"Successfully deleted product {guid}");
                return NoContent();
            }
            return BadRequest();
        }
    }
}
