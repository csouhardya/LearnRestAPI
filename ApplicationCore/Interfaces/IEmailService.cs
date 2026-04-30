using ApplicationCore.Models;

namespace ApplicationCore.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(List<Order> orders);
    }
}
