using ApplicationCore.Models;
using MediatR;

namespace ApplicationCore.Products.Get
{
    public record GetProductsQuery : IRequest<List<Product>>;
    public record GetProductsQueryBySearchTerm(string searchTerm): IRequest<List<Product>>;
}
