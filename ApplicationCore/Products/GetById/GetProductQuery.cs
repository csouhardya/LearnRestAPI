using ApplicationCore.Models;
using MediatR;

namespace ApplicationCore.Products.GetById
{
    /// <summary>
    /// Request to retrieve a single product by its integer Id.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    public record GetProductQuery(Guid guid): IRequest<Product>;

}
