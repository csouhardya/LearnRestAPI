using ApplicationCore.Models;

namespace ApplicationCore.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponse> ValidateCredentialsAsync(LoginRequest request);
        Task<ResponseValidity> RegisterUserAsync(User request);
    }
}
