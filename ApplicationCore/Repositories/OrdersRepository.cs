using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Serilog;
using System.Data;

namespace ApplicationCore.Repositories
{
    public class OrdersRepository(IConnectionProvider connectionProvider, ILogger logger) : IOrdersRepository
    {
        private readonly IConnectionProvider _connetionProvider = connectionProvider;
        private readonly ILogger _logger = logger;
        public async Task<int> CreateOrdersAsync(List<Order> orders)
        {
            await using var conn = await _connetionProvider.ConnectAsync();
            try
            {
                var tempOrders = CreateTableValueParam(orders);
                var result = await conn.ExecuteAsync("spCreateOrders", new
                {
                    tempOrders = tempOrders.AsTableValuedParameter("OrderTVP")
                },
                commandType: CommandType.StoredProcedure);

                return result;
            }
            catch (SqlException sqe)
            {
                _logger.Error($"Sql exception occured. Ex: {sqe}");
                return 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"Unable to create new orders due to error. Ex {ex}");
                return 0;
            }
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            await using var conn = await _connetionProvider.ConnectAsync();
            try
            {
                var result = await conn.QueryAsync<Order>("spGetOrders", commandType: CommandType.StoredProcedure);
                return [.. result];
            }
            catch(SqlException sqe)
            {
                _logger.Error($"Sql exception occured. Ex: {sqe}");
                return [];
            }
            catch(Exception ex)
            {
                _logger.Error($"Unable to create new orders due to error. Ex {ex}");
                return [];
            }
        }
         
        public async Task<int> UpdateOrdersAsync(List<Order> orders)
        {
            await using var conn = await _connetionProvider.ConnectAsync();
            try
            {
                var tempOrders = CreateTableValueParam(orders);
                var result = await conn.ExecuteAsync("spUpdateOrders", new
                {
                    tempOrders = tempOrders.AsTableValuedParameter("OrderTVP")
                },
                commandType: CommandType.StoredProcedure);

                return result;
            }
            catch (SqlException sqe)
            {
                _logger.Error($"Sql exception occured. Ex: {sqe}");
                return 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"Unable to create new orders due to error. Ex {ex}");
                return 0;
            }
        }

        private DataTable CreateTableValueParam(List<Order> orders)
        {
            var tempOrders = new DataTable();
            tempOrders.Columns.Add(new DataColumn("guid"));
            tempOrders.Columns.Add(new DataColumn("count"));
            tempOrders.Columns.Add(new DataColumn("total_amount"));
            tempOrders.Columns.Add(new DataColumn("product_guid"));
            tempOrders.Columns.Add(new DataColumn("product_name"));
            tempOrders.Columns.Add(new DataColumn("status"));
            tempOrders.Columns.Add(new DataColumn("tracking_id"));
            tempOrders.Columns.Add(new DataColumn("user_guid"));
            tempOrders.Columns.Add(new DataColumn("username"));
            tempOrders.Columns.Add(new DataColumn("email_address"));

            orders.ForEach(order => tempOrders.Rows.Add(
                    order.guid,
                    order.Count,
                    order.TotalAmount,
                    order.ProductGuid,
                    order.ProductName,
                    (int)order.Status,
                    string.IsNullOrEmpty(order.TrackingId) ? DBNull.Value : order.TrackingId,
                    order.UserGuid,
                    order.Username,
                    order.EmailAddress
                ));
            return tempOrders;
        }
    }
}
