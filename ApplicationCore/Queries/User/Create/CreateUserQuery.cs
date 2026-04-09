using MediatR;
using ApplicationCore.Models;

namespace ApplicationCore.Queries.User.Create
{
    public record CreateUserQuery(Models.User user): IRequest<RegisterResponse>;
}
