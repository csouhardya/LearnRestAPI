using ApplicationCore.Models;
using MediatR;

namespace ApplicationCore.Products.Update
{
    public record UpdateQueryAsync(Product product): IRequest<bool>;
}
