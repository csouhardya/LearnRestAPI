using ApplicationCore.Models;
using MediatR;

namespace ApplicationCore.Queries.User.Get
{
    public record GetUserQuery(LoginRequest request): IRequest<LoginResponse>;
}
