using Snoozle.Abstractions;
using System.Data;

namespace Snoozle.SqlServer.Implementation
{
    public interface ISqlPropertyConfiguration : IPropertyConfiguration
    {
        SqlDbType? SqlDbType { get; set; }

        string ColumnName { get; set; }
    }
}
