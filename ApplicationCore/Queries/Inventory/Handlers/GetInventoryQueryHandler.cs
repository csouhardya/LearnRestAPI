using Model = ApplicationCore.Models;
using ApplicationCore.Queries.Inventory.Get;
using MediatR;
using Serilog;
using ApplicationCore.Interfaces;

namespace ApplicationCore.Queries.Inventory.Handlers
{
    public class GetInventoryQueryHandler(ILogger logger, IInventoryRepository inventoryRepository) : IRequestHandler<GetInventoryQuery, List<Model.Inventory>>, IRequestHandler<GetInventoryByPIdQuery, Model.Inventory>
    {
        private ILogger _logger = logger;
        private IInventoryRepository _inventoryRepository = inventoryRepository;
        public async Task<List<Model.Inventory>> Handle(GetInventoryQuery request, CancellationToken cancellationToken)
        {
            _logger.Information("Sending data to repository");
            var result = await _inventoryRepository.GetInventoriesAsync();
            if(result != null && result.Count > 0)
            {
                _logger.Information("Successfully fetched inventory data");
                return result;
            }
            _logger.Warning("Unable to fetch inventory data");
            return [];
        }

        public async Task<Model.Inventory> Handle(GetInventoryByPIdQuery request, CancellationToken cancellationToken)
        {
            _logger.Information("Sending data to repository");
            var result = await _inventoryRepository.GetInventoryByPIdAsync(request.guid);
            if (result != null && result.ProductGuid != default)
            {
                _logger.Information("Successfully fetched inventory data");
                return result;
            }
            _logger.Warning("Unable to fetch inventory data");
            return new Model.Inventory();
        }
    }
}
