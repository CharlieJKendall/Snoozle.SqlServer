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
            SqlConnection?.Dispose();
        }
    }
}
