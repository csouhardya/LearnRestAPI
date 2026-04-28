using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;
namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(ILogger logger, IOrderService orderService) : ControllerBase
    {
        private readonly ILogger _logger = logger;
        private readonly IOrderService _orderService = orderService;

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            _logger.Information("Fetching all orders");
            var result = await _orderService.GetOrdersAsync();
            if(result == null && result?.Count > 0)
            {
                _logger.Error("Unable to fetch orders");
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("BySearch")]
        public async Task<IActionResult> GetProductsBySearchAsync([FromQuery] OrderSearchTerm searchTerm)
        {
            _logger.Information("Fetching orders as per search");
            var result = await _orderService.GetOrderBySearchTermAsync(searchTerm);
            if (result == null)
            {
                _logger.Error("Unable to fetch orders as per search term");
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateProductsAsync([FromBody] List<Order> orders)
        {
            if(orders == null || orders.Count < 1)
            {
                _logger.Error("Input products cannot be null");
                return BadRequest();
            }
            var uniqueOrderGuids = orders.Select(_ => _.guid).Distinct();
            _logger.Information($"Updating products: {uniqueOrderGuids}");

            var result = await _orderService.UpdateOrdersAsync(orders);
            if(result.IsValid)
            {
                _logger.Information($"Updating products: {uniqueOrderGuids}");
                return NoContent();
            }
            _logger.Error("Something went wrong, updating orders");
            return BadRequest();
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateProductsAsync([FromBody] List<Order> orders)
        {
            if(orders == null || orders.Count < 1)
            {
                _logger.Error("Input products can not be null");
                return BadRequest();
            }
            _logger.Information($"Creating {orders.Count} new orders");

            var result = await _orderService.CreateOrdersAsync(orders);
            if(result.IsValid)
            {
                _logger.Information($"Created {orders.Count} new orders. ");
                return Created();
            }
            _logger.Error("Something went wrong, creating orders");
            return BadRequest();
        }
    }
}
