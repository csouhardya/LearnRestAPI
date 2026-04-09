using ApplicationCore.Models;

namespace ApplicationCore.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponse> ValidateCredentialsAsync(LoginRequest request);
        Task<RegisterResponse> RegisterUserAsync(User request);
    }
}
