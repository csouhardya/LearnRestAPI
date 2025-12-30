using ApplicationCore.Models;
using MediatR;

namespace ApplicationCore.Products.Get
{
    public record GetProductsQuery : IRequest<List<Product>>;
    public record GetProductsQueryBySearchTerm(string? searchTerm, string? sortBy, string? sortOrder, int? page, int? pageSize): IRequest<PageList<Product>>;
}
