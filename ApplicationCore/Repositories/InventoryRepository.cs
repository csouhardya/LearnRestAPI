using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Dapper;
using Serilog;
using System.Data;

namespace ApplicationCore.Repositories
{
    public class InventoryRepository(ILogger logger, IConnectionProvider connectionProvider) : IInventoryRepository
    {
        private ILogger _logger = logger;
        private IConnectionProvider _connectionProvider = connectionProvider;
        public async Task<int> CreateInventoryAsync(Inventory inventory)
        {
            await using var conn = await _connectionProvider.ConnectAsync();
            try
            {
                var result = await conn.ExecuteAsync("spAddInventory", new
                {
                    @product_guid = inventory.ProductGuid,
                    @quantity = inventory.AbsoluteCount
                }, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"Unable to create new orders due to error. Ex {ex}");
                return 0;
            }
        }

        public async Task<List<Inventory>> GetInventoriesAsync()
        {
            await using var conn = await _connectionProvider.ConnectAsync();
            try
            {
                var result = await conn.QueryAsync<Inventory>("spGetInventories", commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"Unable to fetch inventories due to error. Ex {ex}");
                return [];
            }
        }

        public async Task<Inventory> GetInventoryByPIdAsync(Guid guid)
        {
            var defaultVal = new Inventory();
            await using var conn = await _connectionProvider.ConnectAsync();
            try
            {
                var result = await conn.QueryFirstOrDefaultAsync<Inventory>("spGetInventoryByProduct",new {
                    @product_guid = guid
                
                }, commandType: CommandType.StoredProcedure);
                return result ?? defaultVal;
            }
            catch (Exception ex)
            {
                _logger.Error($"Unable to fetch inventory for product {guid} due to error. Ex {ex}");
                return defaultVal;
            }
        }

        public async Task<int> UpdateInventoriesAsync(List<Inventory> inventories)
        {
            await using var conn = await _connectionProvider.ConnectAsync();
            try
            {
                var tempInventory = new DataTable();
                tempInventory.Columns.Add(new DataColumn("product_guid"));
                tempInventory.Columns.Add(new DataColumn("quantity_to_subtract"));
                tempInventory.Columns.Add(new DataColumn("absolute_quantity"));

                inventories.ForEach(inv => tempInventory.Rows.Add(inv.ProductGuid,
                                              inv.CountToSubtract ?? (object)DBNull.Value,
                                              inv.AbsoluteCount ?? (object)DBNull.Value
                                            ));
                var result = await conn.ExecuteAsync("spUpdateBulkInventory", new
                {
                    tempInventory = tempInventory.AsTableValuedParameter("InventoryTVP")
                }, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch(Exception ex)
            {
                _logger.Error($"Unable to update inventory due to error. Ex {ex}");
                return 0;
            }           
        }
    }
}
