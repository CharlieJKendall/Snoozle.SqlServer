using Snoozle.Exceptions;
using Snoozle.SqlServer.Internal.Wrappers;
using System.Data;

namespace Snoozle.SqlServer.Internal
{
    public class SqlClassProvider : ISqlClassProvider
    {
        public IDatabaseCommand CreateSqlCommand(string sql, IDatabaseConnection databaseConnection)
        {
            ExceptionHelper.ArgumentNull.ThrowIfNecessary(databaseConnection, nameof(databaseConnection));
            ExceptionHelper.ArgumentNull.ThrowIfNecessary(sql, nameof(sql));

            return new DatabaseCommand(sql, databaseConnection);
        }

        public IDatabaseConnection CreateSqlConnection(string connectionString)
        {
            ExceptionHelper.ArgumentNull.ThrowIfNecessary(connectionString, nameof(connectionString));

            return new DatabaseConnection(connectionString);
        }

        public IDatabaseCommandParameter CreateDatabaseCommandParameter(string parameterName, object value, SqlDbType sqlDbType, bool isNullable)
        {
            return new DatabaseCommandParameter(parameterName, value, sqlDbType, isNullable);
        }
    }
}
