using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;


namespace ApplicationCore.DataAccess
{
    /// <summary>
    /// Repository providing data access methods for products using ADO.NET and Dapper.
    /// </summary>
    public class ProductsRepository(IConnectionProvider connectionProvider) : IProductsRepository
    {
        private readonly IConnectionProvider _connectionProvider = connectionProvider;

        /// <summary>
        /// Retrieves all products using ADO.NET by executing the stored procedure <c>spGetAllProducts</c>.
        /// Maps reader columns (id, guid, name, sku, currency, amount) into <see cref="Product"/> instances.
        /// </summary>
        /// <returns>A task that resolves to the list of products.</returns>
        public async Task<List<Product>> GetAllAsync() // using ADO .NET
        {
            var products = new List<Product>();

            await using var conn = await _connectionProvider.ConnectAsync();
            await using var cmd = new SqlCommand("spGetAllProducts", conn) { CommandType = CommandType.StoredProcedure };

            await using var reader = await cmd.ExecuteReaderAsync();
            var ordGuid = reader.GetOrdinal("guid");
            var ordName = reader.GetOrdinal("name");
            var ordSku = reader.GetOrdinal("sku");
            var ordCurrency = reader.GetOrdinal("currency");
            var ordAmount = reader.GetOrdinal("amount");

            while (await reader.ReadAsync())
            {
                var product = new Product
                {
                    Guid = reader.GetGuid(ordGuid),
                    Name = reader.GetString(ordName),
                    Sku = reader.GetString(ordSku),
                    Currency = reader.GetString(ordCurrency),
                    Amount = reader.GetDecimal(ordAmount)
                };
                products.Add(product);
            }
            return products;
        }

        /// <summary>
        /// Retrieves a single product by its GUID using Dapper and the stored procedure <c>spGetProductByIdentifier</c>.
        /// </summary>
        /// <param name="guid">Product identifier (GUID).</param>
        /// <returns>The product when found; otherwise null.</returns>
        public async Task<Product> GetAsync(Guid guid) // using Dapper
        {
            await using var conn = await _connectionProvider.ConnectAsync();
            var result = await conn.QuerySingleOrDefaultAsync<Product>("spGetProductByIdentifier", new
            {
                guid = guid
            },
            commandType: CommandType.StoredProcedure);
            return result;

        }

        /// <summary>
        /// Inserts a new product using Dapper. (Implementation pending)
        /// </summary>
        /// <param name="product">Product to insert.</param>
        public async Task<int> AddAsync(Product product)
        {
            await using var conn = await _connectionProvider.ConnectAsync();
            var result = await conn.ExecuteAsync("spAddProduct", new
            {
                guid = product.Guid,
                name = product.Name,
                sku = product.Sku,
                currency = product.Currency,
                amount = product.Amount
            },
            commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<int> UpdateAsync(Product product)
        {
            await using var conn = await _connectionProvider.ConnectAsync();
            var result = await conn.ExecuteAsync("spUpdateProduct", new
            {
                guid = product.Guid,
                name = product.Name,
                sku = product.Sku,
                currency = product.Currency,
                amount = product.Amount
            },
            commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<int> DeleteAsync(Guid guid)
        {
            await using var conn = await _connectionProvider.ConnectAsync();
            var result = await conn.ExecuteAsync("spDeleteProduct", new
            {
                guid = guid
            },
            commandType: CommandType.StoredProcedure);
            return result;
        }

    }
}
