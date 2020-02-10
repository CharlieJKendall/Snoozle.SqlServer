using Snoozle.Abstractions;
using Snoozle.Exceptions;
using Snoozle.SqlServer.Implementation;
using System.Data;
using System.Text.RegularExpressions;

namespace Snoozle.SqlServer
{
    public static class ModelConfigurationBuilderExtensions
    {
        private static Regex _tableNameRegex = new Regex(@"^[\p{L}_][\p{L}\p{N}@$#_]{0,127}$");

        public static IModelConfigurationBuilder<ISqlModelConfiguration> HasTableName(
            this IModelConfigurationBuilder<ISqlModelConfiguration> @this,
            string tableName)
        {
            ExceptionHelper.ArgumentNull.ThrowIfNecessary(tableName, nameof(tableName));
            ExceptionHelper.Argument.ThrowIfTrue(!_tableNameRegex.IsMatch(tableName), $"'{tableName}' is an invalid table name.", nameof(tableName));

            @this.ModelConfiguration.TableName = tableName;

            return @this;
        }
    }

    public static class PropertyConfigurationBuilderExtensions
    {
        private static Regex _columnNameRegex = new Regex(@"^[a-zA-Z_][a-zA-Z0-9_ ]*$");

        public static IPropertyConfigurationBuilder<TProperty, ISqlPropertyConfiguration> HasSqlDbType<TProperty>(
            this IPropertyConfigurationBuilder<TProperty, ISqlPropertyConfiguration> @this,
            SqlDbType sqlDbType)
        {
            @this.PropertyConfiguration.SqlDbType = sqlDbType;

            return @this;
        }

        public static IPropertyConfigurationBuilder<TProperty, ISqlPropertyConfiguration> HasColumnName<TProperty>(
            this IPropertyConfigurationBuilder<TProperty, ISqlPropertyConfiguration> @this,
            string columnName)
        {
            ExceptionHelper.ArgumentNull.ThrowIfNecessary(columnName, nameof(columnName));
            ExceptionHelper.Argument.ThrowIfTrue(!_columnNameRegex.IsMatch(columnName), $"'{columnName}' is an invalid column name.", nameof(columnName));

            @this.PropertyConfiguration.ColumnName = columnName;

            return @this;
        }
    }
}
