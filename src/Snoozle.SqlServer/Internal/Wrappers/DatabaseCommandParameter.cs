using System;
using System.Data;
using System.Data.SqlClient;

namespace Snoozle.SqlServer.Internal.Wrappers
{
    public class DatabaseCommandParameter : IDatabaseCommandParameter
    {
        public SqlParameter SqlParameter { get; }

        public DatabaseCommandParameter(string parameterName, object value, SqlDbType sqlDbType, bool isNullable)
        {
            SqlParameter = new SqlParameter(parameterName, value ?? DBNull.Value)
            {
                SqlDbType = sqlDbType,
                IsNullable = isNullable
            };
        }
    }
}
