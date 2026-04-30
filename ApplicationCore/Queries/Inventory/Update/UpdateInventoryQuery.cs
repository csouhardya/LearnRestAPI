using ApplicationCore.Models;
using MediatR;
using Model = ApplicationCore.Models;
namespace ApplicationCore.Queries.Inventory.Update
{
    public record UpdateInventoryQuery(List<Model.Inventory> Inventories) : IRequest<ResponseValidity>;
}
