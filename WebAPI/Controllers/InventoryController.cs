using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController(ILogger logger, IInventoryService inventoryService, IEmailService es) : ControllerBase
    {
        private readonly ILogger _logger = logger;
        private readonly IInventoryService _inventoryService = inventoryService;
        private readonly IEmailService es = es;

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllInventoriesAsync()
        {
            _logger.Information("Fetching all inventory items");
            var result = await _inventoryService.GetInventories();
            if (result == null && result?.Count > 0)
            {
                _logger.Error("Unable to fetch inventory items");
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("ByProductGuid")]
        public async Task<IActionResult> GetInventoryByProductAsync([FromQuery] Guid guid)
        {
            _logger.Information($"Fetching inventory of product: {guid}");
            var result = await _inventoryService.GetInventoryByGuid(guid);
            if (result == null)
            {
                _logger.Error($"Unable to fetch inventory of product: {guid}");
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdaOrderAsync([FromBody] Inventory inventory)
        {
            if (inventory == null || inventory.ProductGuid == default || inventory.AbsoluteCount == null)
            {
                _logger.Error("Input values cannot be null");
                return BadRequest();
            }
            
            _logger.Information($"Updating products inventory: {inventory.ProductGuid}");

            List<Inventory> invLst = [];
            var result = await _inventoryService.UpdateInventoriesAsync(invLst.Append(inventory).ToList());
            if (result.IsValid)
            {
                _logger.Information($"Updating products inventory: {inventory.ProductGuid}");
                return NoContent();
            }
            _logger.Error("Something went wrong, updating inventory");
            return BadRequest();
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateOrdersAsync([FromBody] Inventory inventory)
        {
            if (inventory == null || inventory.ProductGuid == default)
            {
                _logger.Error("Input products inventory can not be null");
                return BadRequest();
            }
            _logger.Information($"Creating inventory item for product {inventory.ProductGuid}");

            var result = await _inventoryService.CreateInventoryAsync(inventory);
            if (result.IsValid)
            {
                _logger.Information($"Created new inventory for product {inventory.ProductGuid} ");
                return Created();
            }
            _logger.Error("Something went wrong, creating inventory item");
            return BadRequest();
        }
    }
}
