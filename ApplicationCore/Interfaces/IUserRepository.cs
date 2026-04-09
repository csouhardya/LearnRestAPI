using ApplicationCore.Models;

namespace ApplicationCore.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(LoginRequest request);
        Task<RegisterResponse> CreateUserAsync(User user);
    }
}
