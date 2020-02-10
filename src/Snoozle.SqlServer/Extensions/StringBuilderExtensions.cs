using System.Text;

namespace Snoozle.SqlServer.Extensions
{
    internal static class StringBuilderExtensions
    {
        public static StringBuilder AppendWhereClause(this StringBuilder @this, string columnName, string parameterName)
        {
            @this.Append(" WHERE [");
            @this.Append(columnName);
            @this.Append("] = ");
            @this.Append(parameterName);
            @this.Append(" ");

            return @this;
        }
    }
}
