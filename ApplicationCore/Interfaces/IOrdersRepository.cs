using ApplicationCore.Models;

namespace ApplicationCore.Interfaces
{
    public interface IOrdersRepository
    {
        Task<List<Order>> GetOrdersAsync();
        Task<int> UpdateOrdersAsync(List<Order> orders);
        Task<int> CreateOrdersAsync(List<Order> orders);
    }
}
