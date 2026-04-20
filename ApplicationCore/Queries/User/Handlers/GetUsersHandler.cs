using ApplicationCore.Helpers;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.Queries.User.Get;
using MediatR;

namespace ApplicationCore.Queries.User.Handlers
{
    public class GetUsersHandler(IUserRepository userRepository, IPasswordHelper passwordHelper) : IRequestHandler<GetUserQuery, LoginResponse>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordHelper _passwordHelper = passwordHelper;
        public async Task<LoginResponse> Handle(GetUserQuery requestQuery, CancellationToken cancellationToken)
        {
            LoginResponse loginResponse = new();
            var user = await _userRepository.GetUserAsync(requestQuery.request);
            if(user == null)
            {
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
