using MediatR;

namespace ApplicationCore.Queries.Products.Delete
{
    public record DeleteProductsQuery(Guid guid): IRequest<bool>;
}
