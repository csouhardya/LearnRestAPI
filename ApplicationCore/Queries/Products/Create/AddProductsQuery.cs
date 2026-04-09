using ApplicationCore.Models;
using MediatR;

namespace ApplicationCore.Queries.Products.Create
{
    public record AddProductsQuery(Product product): IRequest<bool>;
}
