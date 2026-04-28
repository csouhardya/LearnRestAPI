using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.Queries.User.Get;
using MediatR;
using Serilog;

namespace ApplicationCore.Queries.User.Handlers
{
    public class GetUsersHandler(IUserRepository userRepository, 
                IPasswordHelper passwordHelper,
                ILogger logger) : IRequestHandler<GetUserQuery, LoginResponse>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordHelper _passwordHelper = passwordHelper;
        private readonly ILogger _logger = logger;
        public async Task<LoginResponse> Handle(GetUserQuery requestQuery, CancellationToken cancellationToken)
        {
            LoginResponse loginResponse = new();

            _logger.Information("Sending login request to repository");
            var user = await _userRepository.GetUserAsync(requestQuery.request);
            if(user == null)
            {
                _logger.Error("No user found with username and password");
                // TODO : validate
                loginResponse.Role = UserRoles.NotRegistered;
                return  loginResponse;
            }

            var isValid = _passwordHelper.Verify(requestQuery.request.Password, user.Credentials.Password);
            loginResponse.IsValid = isValid;
            loginResponse.Role = user.Role;
            return loginResponse;
            


        }
    }
}
