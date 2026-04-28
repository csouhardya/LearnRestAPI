using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.Queries.Orders.Create;
using MediatR;
using Serilog;

namespace ApplicationCore.Queries.Orders.Handlers
{
    public class AddOrdersHandler(IOrdersRepository orderRepository,
                        ILogger logger,
                        ICachingService cachingService) : IRequestHandler<AddOrdersQuery, ResponseValidity>
    {
        private readonly IOrdersRepository _ordersRepository = orderRepository;
        private readonly ILogger _logger = logger;
        private readonly ICachingService _cachingService = cachingService; // Not using caching because many user may create  new orders simultaneously. Write heavy
        public async Task<ResponseValidity> Handle(AddOrdersQuery request, CancellationToken cancellationToken)
        {
            var respValidity = new ResponseValidity();
            _logger.Information("Sending new orders to repository.");

            var guid = Guid.NewGuid();
            foreach (var item in request.orders)
            {
                item.guid = guid;
            }
            var resp = await _ordersRepository.CreateOrdersAsync(request.orders);
            if(resp > 0)
            {
                _logger.Information("Successfully created new orders");
                respValidity.IsValid = true;
                return respValidity;
            }
            respValidity.IsValid = false;
            respValidity.ErrorMessage = "Unable to create new Orders"; // TODO : make validationResult
            return respValidity;
        }
    }
}
