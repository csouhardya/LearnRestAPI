using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.Queries.User.Create;
using ApplicationCore.Queries.User.Get;
using MediatR;

namespace ApplicationCore.Services
{
    public class UserService(ISender sender) : IUserService
    {
        private ISender _sender = sender;
        public async Task<LoginResponse> ValidateCredentialsAsync(LoginRequest request)
        {
            var query = new GetUserQuery(request);
            var result = await _sender.Send(query);
            return result;
        }

        public async Task<RegisterResponse> RegisterUserAsync(User user)
        {
            var query = new CreateUserQuery(user);
            var result = await _sender.Send(query);
            return result;
        }
    }
}
