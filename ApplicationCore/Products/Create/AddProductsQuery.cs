using ApplicationCore.Models;
using MediatR;

namespace ApplicationCore.Products.Create
{
    public record AddProductsQuery(Product product): IRequest<bool>;
}
