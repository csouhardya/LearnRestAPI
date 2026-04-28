using ApplicationCore.Models;
using MediatR;

namespace ApplicationCore.Queries.Orders.Get
{
    public record GetOrdersQuery() : IRequest<List<Order>>;
    public record GetOrdersBySearchQuery(OrderSearchTerm searchTerm) : IRequest<PageList<Order>>;
}