using ApplicationCore.Interfaces;
using ApplicationCore.Misc;
using ApplicationCore.Models;
using ApplicationCore.Queries.Orders.Get;
using ApplicationCore.Queries.Products.Get;
using MediatR;
using Serilog;

namespace ApplicationCore.Queries.Orders.Handlers
{
    public class GetOrdersHandler(IOrdersRepository orderRepository,
                        ILogger logger,
                        ICachingService cachingService) : IRequestHandler<GetOrdersQuery, List<Order>>, IRequestHandler<GetOrdersBySearchQuery, PageList<Order>?>
    {
        private readonly IOrdersRepository _ordersRepository = orderRepository;
        private readonly ILogger _logger = logger;
        private readonly ICachingService _cachingService = cachingService;
        public async Task<List<Order>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var data = await GetAllOrders();
            return data;
        }

        public async Task<PageList<Order>?> Handle(GetOrdersBySearchQuery request, CancellationToken cancellationToken)
        {
            var orders = await GetAllOrders();
            var searchTerm = request.searchTerm;
            if(orders == null || orders.Count < 1)
            {
                _logger.Warning("No orders found");
                return null;
            }

            if(!string.IsNullOrEmpty(searchTerm.Username))
                orders = orders.Where(_ => _.Username == request.searchTerm.Username).ToList();

            if (searchTerm.OrderDate == default(DateOnly))
                orders = orders.Where(_ =>  _.CreatedDate == request.searchTerm.OrderDate).ToList();

            if(searchTerm.OrderGuid != null)
                orders = orders.Where(_ => _.guid == request.searchTerm.OrderGuid).ToList();

            if(searchTerm.Status != null)
                orders = orders.Where(_ => _.Status == request.searchTerm.Status).ToList();

            if(!string.IsNullOrEmpty(searchTerm.EmailAddress))
                orders = orders.Where(_ => _.EmailAddress == request.searchTerm.EmailAddress).ToList();

            if(searchTerm.ProductGuid is null)
                orders = orders.Where(_ => _.ProductGuid == request.searchTerm.ProductGuid).ToList();

            if(searchTerm.TotalAmount is null)
                orders = orders.Where(_ => _.TotalAmount == request.searchTerm.TotalAmount).ToList();

            if(!string.IsNullOrEmpty(searchTerm.TrackingId))
                orders = orders.Where(_ => _.TrackingId == request.searchTerm.TrackingId).ToList();

            _logger.Information($"Page count is {searchTerm.PageNumber}");
            _logger.Information($"Page size is {searchTerm.PageSize}");

            //TODO : sorting to be  implemented
            var response = PaginationHandler<Order>.Paginate(orders.AsQueryable(), searchTerm.PageNumber, searchTerm.PageSize);
            return response;

        }

        private async Task<List<Order>> GetAllOrders()
        {
            List<Order>? data;
            data = _cachingService.GetData<List<Order>>(Constants.AllOrdersCacheKey);
            if (data is null)
            {
                _logger.Information("Cache is empty, fetching data from database");
                data = await _ordersRepository.GetOrdersAsync();
                _cachingService.SetData(Constants.AllOrdersCacheKey, data);
            }
            return data;
        }


    }
}
