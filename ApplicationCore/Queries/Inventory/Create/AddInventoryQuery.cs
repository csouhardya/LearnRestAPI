using ApplicationCore.Models;
using MediatR;
using Model = ApplicationCore.Models;

namespace ApplicationCore.Queries.Inventory.Create
{
    public record AddInventoryQuery(Model.Inventory inventory) : IRequest<ResponseValidity>;
}
