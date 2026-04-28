using ApplicationCore.Models;
using MediatR;

namespace ApplicationCore.Queries.Orders.Update
{
    public record UpdateOrdersQuery(List<Order> orders) : IRequest<ResponseValidity>;
}
