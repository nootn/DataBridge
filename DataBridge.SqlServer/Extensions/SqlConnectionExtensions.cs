using System.Data;
using System.Data.SqlClient;

namespace DataBridge.SqlServer.Extensions
{
    public static class SqlConnectionExtensions
    {
        /// <summary>
        /// Ensures the connection is open if it is not already
        /// </summary>
        /// <param name="item">The SQL connection to check</param>
        public static void EnsureOpen(this SqlConnection item)
        {
            if (item.State != ConnectionState.Open)
            {
                item.Open();
            }
        }
    }
}