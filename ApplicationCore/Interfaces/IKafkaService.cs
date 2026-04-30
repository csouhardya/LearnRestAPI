using ApplicationCore.Models;

namespace ApplicationCore.Interfaces
{
    public interface IKafkaService
    {
        Task ProduceOrderAsync(List<Order> order);
    }
}
