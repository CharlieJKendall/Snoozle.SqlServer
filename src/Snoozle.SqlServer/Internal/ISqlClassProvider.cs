using Snoozle.SqlServer.Internal.Wrappers;
using System.Data;

namespace Snoozle.SqlServer.Internal
{
    public interface ISqlClassProvider
    {
        IDatabaseCommand CreateSqlCommand(string sql, IDatabaseConnection databaseConnection);

        IDatabaseConnection CreateSqlConnection(string connectionString);

        IDatabaseCommandParameter CreateDatabaseCommandParameter(string parameterName, object value, SqlDbType sqlDbType, bool isNullable);
    }
}
