using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.Queries.Inventory.Create;
using ApplicationCore.Queries.Inventory.Get;
using ApplicationCore.Queries.Inventory.Update;
using MediatR;
using Serilog;

namespace ApplicationCore.Services
{
    public class InventoryService(ISender sender, ILogger logger) : IInventoryService
    {
        private readonly ISender _sender = sender;
        private readonly ILogger _logger = logger;
        public async Task<ResponseValidity> CreateInventoryAsync(Inventory inventory)
        {
            var query = new AddInventoryQuery(inventory);
            _logger.Information("Sending query to handler");
            var result = await _sender.Send(query);
            return result;
        }

        public async Task<List<Inventory>> GetInventories()
        {
            var query = new GetInventoryQuery();
            _logger.Information("Sending query to handler");
            var result = await _sender.Send(query);
            return result;
        }

        public async Task<Inventory> GetInventoryByGuid(Guid guid)
        {
            var query = new GetInventoryByPIdQuery(guid);
            _logger.Information("Sending query to handler");
            var result = await _sender.Send(query);
            return result;
        }

        public async Task<ResponseValidity> UpdateInventoriesAsync(List<Inventory> inventories)
        {
            var query = new UpdateInventoryQuery(inventories);
            _logger.Information("Sending query to handler");
            var result = await _sender.Send(query);
            return result;
        }
    }
}
