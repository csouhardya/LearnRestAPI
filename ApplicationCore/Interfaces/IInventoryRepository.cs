using ApplicationCore.Models;

namespace ApplicationCore.Interfaces
{
    public interface IInventoryRepository
    {
        Task<int> UpdateInventoriesAsync(List<Inventory> inventories);
        Task<int> CreateInventoryAsync(Inventory inventory);
        Task<List<Inventory>> GetInventoriesAsync();
        Task<Inventory> GetInventoryByPIdAsync(Guid guid);
    }
}
