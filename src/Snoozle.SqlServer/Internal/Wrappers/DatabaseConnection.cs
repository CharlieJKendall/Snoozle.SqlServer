using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Snoozle.SqlServer.Internal.Wrappers
{
    public class DatabaseConnection : IDatabaseConnection
    {
        public SqlConnection SqlConnection { get; }

        public DatabaseConnection(string connectionString)
        {
            SqlConnection = new SqlConnection(connectionString);
        }

        public Task OpenAsync()
        {
            return SqlConnection.OpenAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                SqlConnection?.Dispose();
            }
        }
    }
}
