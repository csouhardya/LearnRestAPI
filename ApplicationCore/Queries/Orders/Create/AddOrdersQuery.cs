using ApplicationCore.Models;
using MediatR;

namespace ApplicationCore.Queries.Orders.Create
{
    public record AddOrdersQuery(List<Order> orders) : IRequest<ResponseValidity>;    
}
