using ApplicationCore.Models;
using MediatR;

namespace ApplicationCore.Queries.Products.Update
{
    public record UpdateQueryAsync(Product product): IRequest<bool>;
}
