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
        public ProductsController(IProductService productService)
        {
            _productsService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productsService.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("SearchTerm")]
        public async Task<IActionResult> GetProductsWithSearchTerm(string? searchTerm, string? sortBy, string? sortOrder, int? page, int? pageSize)
        {
            //TODO validate if page != null then pageSize is required and minimum number should be 1
            var products = await _productsService.GetProductsAsync(searchTerm, sortBy, sortOrder, page, pageSize);
            return Ok(products);
        }

        [HttpGet]
        [Route("Guid")]
        public async Task<IActionResult> GetProductById(string guid)
        {
            // TODO to be implemented
            var products = await _productsService.GetProductsAsync();
            return Ok(products);
        }
    }
}
