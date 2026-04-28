using ApplicationCore.Interfaces;
using ApplicationCore.Misc;
using ApplicationCore.Models;
using ApplicationCore.Queries.Orders.Update;
using MediatR;
using Serilog;

namespace ApplicationCore.Queries.Orders.Handlers
{
    public class UpdateOrdersHandler(IOrdersRepository orderRepository,
                        ILogger logger,
                        ICachingService cachingService) : IRequestHandler<UpdateOrdersQuery, ResponseValidity>
    {
        private readonly IOrdersRepository _ordersRepository = orderRepository;
        private readonly ILogger _logger = logger;
        // using caching so that admin updated values can reflect. Update is done by admins only.
        private readonly ICachingService _cachingService = cachingService;

        public async Task<ResponseValidity> Handle(UpdateOrdersQuery request, CancellationToken cancellationToken)
        {
            var respValidity = new ResponseValidity();
            _logger.Information("Sending order updation to repository.");
            var resp = await _ordersRepository.UpdateOrdersAsync(request.orders);
            if (resp > 0)
            {
                _logger.Information("Successfully updated new orders");

                var oldOrders = _cachingService.GetData<List<Order>>(Constants.AllOrdersCacheKey);
                if(oldOrders != null && oldOrders.Count > 0)
                {
                    _logger.Information("Updating Cache with updated values");
                    var tempDict = new Dictionary<Guid, (OrderStatus, string?)>();
                    foreach (var item in request.orders)
                    {
                        // form a dictionary with the guid and updated values, same guid may exist but will always have same updated values.
                        // because same order guid will be shipped together.
                        tempDict[(Guid)item.guid!] = (item.Status, item.TrackingId); 
                    }

                    // update the values in old list
                    foreach (var oldItem in oldOrders)
                    {
                        if(tempDict.TryGetValue((Guid)oldItem.guid!, out var newValue))
                        {
                            oldItem.Status = newValue.Item1; // Item1 = orderStatus from tuple
                            oldItem.TrackingId = newValue.Item2; // Item2 = trackingId from tuple
                        }
                    }

                    // old Orders are updated
                    _cachingService.ReInsertData(Constants.AllOrdersCacheKey, oldOrders);
                }
                respValidity.IsValid = true;
                return respValidity;
            }
            respValidity.IsValid = false;
            respValidity.ErrorMessage = "Unable to update new Orders"; // TODO : make validationResult
            return respValidity;
        }
    }
}
