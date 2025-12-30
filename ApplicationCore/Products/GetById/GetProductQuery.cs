using ApplicationCore.Models;
using MediatR;

namespace ApplicationCore.Products.GetById
{
    public record GetProductQuery(int id): IRequest<Product>;

}
