using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Snoozle.SqlServer.Internal.Wrappers
{
    public class DatabaseCommand : IDatabaseCommand
    {
        private readonly SqlCommand _sqlCommand;

        public DatabaseCommand(string sql, IDatabaseConnection databaseConnection)
        {
            _sqlCommand = new SqlCommand(sql, databaseConnection.SqlConnection);
        }

        public void Dispose()
        {
            _sqlCommand?.Dispose();
        }

        public Task<int> ExecuteNonQueryAsync()
        {
            return _sqlCommand.ExecuteNonQueryAsync();
        }

        public async Task<IDatabaseResultReader> ExecuteReaderAsync()
        {
            return new DatabaseResultReader(await _sqlCommand.ExecuteReaderAsync());
        }

        public void AddParameter(IDatabaseCommandParameter databaseCommandParameter)
        {
            _sqlCommand.Parameters.Add(databaseCommandParameter.SqlParameter);
        }

        public void AddParameters(IEnumerable<IDatabaseCommandParameter> databaseCommandParameters)
        {
            _sqlCommand.Parameters.AddRange(databaseCommandParameters.Select(param => param.SqlParameter).ToArray());
        }
    }
}
