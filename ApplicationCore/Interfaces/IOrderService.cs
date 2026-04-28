using ApplicationCore.Models;

namespace ApplicationCore.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersAsync();
        Task<PageList<Order>> GetOrderBySearchTermAsync(OrderSearchTerm searchTerm);
        Task<ResponseValidity> CreateOrdersAsync(List<Order> orders);
        Task<ResponseValidity> UpdateOrdersAsync(List<Order> orders);
    }
}
