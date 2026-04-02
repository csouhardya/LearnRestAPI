using Microsoft.Data.SqlClient;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// Provides SQL connections from configuration or environment.
    /// </summary>
    public interface IConnectionProvider
    {
        /// <summary>
        /// Returns an open <see cref="SqlConnection"/> instance.
        /// </summary>
        Task<SqlConnection> ConnectAsync();
    }
}
