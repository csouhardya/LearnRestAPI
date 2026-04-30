using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.Queries.Inventory.Create;
using MediatR;
using Serilog;

namespace ApplicationCore.Queries.Inventory.Handlers
{
    public class AddInventoryHandler(ILogger logger, IInventoryRepository inventoryRepository) : IRequestHandler<AddInventoryQuery, ResponseValidity>
    {
        private ILogger _logger = logger;
        private IInventoryRepository _inventoryRepository = inventoryRepository;
        public async Task<ResponseValidity> Handle(AddInventoryQuery request, CancellationToken cancellationToken)
        {
            ResponseValidity resp = new();
            _logger.Information("Sending inventory creation items to repository");
            var result = await _inventoryRepository.CreateInventoryAsync(request.inventory);
            if (result > 0)
            {
                _logger.Information($"Successfully created inventories with product guids: {request.inventory.ProductGuid}");
                resp.IsValid = true;
                return resp;
            }
            _logger.Error($"Unable to create product: {request.inventory.ProductGuid} ");
            resp.IsValid = false;
            resp.ErrorMessage = "Unable to create inventory item"; //TODO : move to validationresult
            return resp;
        }
    }
}
