using Snoozle.Abstractions;
using System.Data;

namespace Snoozle.SqlServer.Implementation
{
    public class SqlPropertyConfiguration : BasePropertyConfiguration, ISqlPropertyConfiguration
    {
        public SqlDbType? SqlDbType { get; set; }

        public string ColumnName { get; set; }
    }
}
