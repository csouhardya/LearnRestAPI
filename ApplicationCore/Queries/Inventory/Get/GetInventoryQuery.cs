using Model = ApplicationCore.Models;
using MediatR;

namespace ApplicationCore.Queries.Inventory.Get
{
    public record GetInventoryQuery() : IRequest<List<Model.Inventory>>;
    public record GetInventoryByPIdQuery(Guid guid) : IRequest<Model.Inventory>;
}
