using MediatR;

namespace ApplicationCore.Products.Delete
{
    public record DeleteProductsQuery(Guid guid): IRequest<bool>;
}
