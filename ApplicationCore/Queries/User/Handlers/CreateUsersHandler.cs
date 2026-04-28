using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.Queries.User.Create;
using MediatR;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Queries.User.Handlers
{
    public class CreateUsersHandler(IUserRepository userRepository,
                        IPasswordHelper passwordHelper,
                        ILogger logger): IRequestHandler<CreateUserQuery, RegisterResponse>
    {
        private IUserRepository _userRepository = userRepository;
        private IPasswordHelper _passwordHelper = passwordHelper;
        private ILogger _logger = logger;

        public async Task<RegisterResponse> Handle(CreateUserQuery query , CancellationToken cancellationToken)
        {
            _logger.Information($"Sending register request to repository");
            var user = query.user;
            Guid guid = Guid.NewGuid();
            var role = UserRoles.User;
            EmailAddressAttribute emailAttr = new();
            RegisterResponse resp = new();

            user.guid = guid;
            user.Role = role;
            _logger.Information($"Generated guid: {user.guid} and role: {user.Role}");

            if (!emailAttr.IsValid(user.Email))
            {
                _logger.Error($"Email address {user.Email} is not valid");
                resp.IsCreated = false;
                resp.ErrorMessage = "Email address is not in correct format"; // TODO : validation result
                return resp;
            }

            if(user.Credentials.Username.Length < 3 && user.Credentials.Username.Length > 30)
            {
                _logger.Error($"Username not in correct length. Length: {user.Credentials.Username.Length}");
                resp.IsCreated = false;
                resp.ErrorMessage = "Username length must be in between 3 and 30"; // TODO : validation result
                return resp;
            }

            // TODO : password format

            user.Credentials.Password = _passwordHelper.Hash(user.Credentials.Password);

            resp = await _userRepository.CreateUserAsync(user);
            return resp;
        }
    }
}
