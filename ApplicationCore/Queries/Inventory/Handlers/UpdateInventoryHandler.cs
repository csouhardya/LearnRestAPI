using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.Queries.Inventory.Update;
using MediatR;
using Serilog;

namespace ApplicationCore.Queries.Inventory.Handlers
{
    public class UpdateInventoryHandler(ILogger logger, IInventoryRepository inventoryRepository) : IRequestHandler<UpdateInventoryQuery, ResponseValidity>
    {
        private ILogger _logger = logger;
        private IInventoryRepository _inventoryRepository = inventoryRepository;
        public async Task<ResponseValidity> Handle(UpdateInventoryQuery request, CancellationToken cancellationToken)
        {
            ResponseValidity resp = new();
            _logger.Information("Sending inventory updation items to repository");
            var result = await _inventoryRepository.UpdateInventoriesAsync(request.Inventories);
            if(result > 0)
            {
                _logger.Information($"Successfully updated inventories with product guids: {request.Inventories.Select(_ => _.ProductGuid).ToList()}");
                resp.IsValid = true;
                return resp;
            }
            _logger.Error($"Unable to update products: {request.Inventories.Select(_ => _.ProductGuid).ToList()}");
            resp.IsValid = false;
            resp.ErrorMessage = "Unable to update inventory"; //TODO : move to validationresult
            return resp;
        }
    }
}
