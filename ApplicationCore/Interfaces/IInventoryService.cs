using ApplicationCore.Models;

namespace ApplicationCore.Interfaces
{
    public interface IInventoryService
    {
        Task<ResponseValidity> CreateInventoryAsync(Inventory inventory);
        Task<ResponseValidity> UpdateInventoriesAsync(List<Inventory> inventories);
        Task<List<Inventory>> GetInventories();
        Task<Inventory> GetInventoryByGuid(Guid guid); // To check before adding in the cart
    }
}
