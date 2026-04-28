using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.Queries.User.Create;
using ApplicationCore.Queries.User.Get;
using MediatR;
using Serilog;

namespace ApplicationCore.Services
{
    public class UserService(ISender sender, ILogger logger) : IUserService
    {
        private ISender _sender = sender;
        private ILogger _logger = logger;
        public async Task<LoginResponse> ValidateCredentialsAsync(LoginRequest request)
        {
            var query = new GetUserQuery(request);
            _logger.Information($"Sending login request to handler. Username: {request.Username}");
            var result = await _sender.Send(query);
            return result;
        }

        public async Task<ResponseValidity> RegisterUserAsync(User user)
        {
            var query = new CreateUserQuery(user);
            _logger.Information($"Sending register request to handler.Username: {user.Credentials.Username}");
            var result = await _sender.Send(query);
            return result;
        }
    }
}
