using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.Queries.Orders.Create;
using ApplicationCore.Queries.Orders.Get;
using ApplicationCore.Queries.Orders.Update;
using MediatR;
using Serilog;

namespace ApplicationCore.Services
{
    public class OrderService(ISender sender, ILogger logger) : IOrderService
    {
        private readonly ISender _sender = sender;
        private readonly ILogger _logger = logger;

        public async Task<ResponseValidity> CreateOrdersAsync(List<Order> orders)
        {
            _logger.Information("Sending orders to handler");
            var query = new AddOrdersQuery(orders);
            var result = await _sender.Send(query);
            return result;
        }

        public async Task<PageList<Order>> GetOrderBySearchTermAsync(OrderSearchTerm searchTerm)
        {
            _logger.Information("Sending orders to handler");
            var query = new GetOrdersBySearchQuery(searchTerm);
            var result = await _sender.Send(query);
            return result;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            _logger.Information("Fetching orders from handler");
            var query = new GetOrdersQuery();
            var result = await _sender.Send(query);
            return result;
        }

        public async Task<ResponseValidity> UpdateOrdersAsync(List<Order> orders)
        {
            _logger.Information("Sending orders to handler");
            var query = new UpdateOrdersQuery(orders);
            var result = await _sender.Send(query);
            return result;
        }
    }
}
