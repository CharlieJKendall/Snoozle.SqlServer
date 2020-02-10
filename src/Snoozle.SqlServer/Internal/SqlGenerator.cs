using Snoozle.SqlServer.Extensions;
using Snoozle.SqlServer.Implementation;
using System.Linq;
using System.Text;

namespace Snoozle.SqlServer.Internal
{
    public class SqlGenerator : ISqlGenerator
    {
        private readonly ISqlParamaterProvider _sqlParamaterProvider;

        public SqlGenerator(ISqlParamaterProvider sqlParamaterProvider)
        {
            _sqlParamaterProvider = sqlParamaterProvider;
        }

        public string SelectAll(ISqlResourceConfiguration config)
        {
            return SelectAllBuilder(config).ToString();
        }

        public string DeleteById(ISqlResourceConfiguration config)
        {
            return new StringBuilder($"DELETE FROM [{config.ModelConfiguration.TableName}]")
                .AppendWhereClause(config.PrimaryIdentifier.ColumnName, _sqlParamaterProvider.GetPrimaryKeyParameterName())
                .ToString();
        }

        private StringBuilder SelectAllBuilder(ISqlResourceConfiguration config)
        {
            StringBuilder stringBuilder = new StringBuilder("SELECT ");
            ISqlPropertyConfiguration[] properties = config.PropertyConfigurationsForRead.ToArray();

            for (int i = 0; i < properties.Length; i++)
            {
                stringBuilder.Append("[");
                stringBuilder.Append(properties[i].ColumnName);
                stringBuilder.Append("] AS [");
                stringBuilder.Append(properties[i].PropertyName);
                stringBuilder.Append("]");

                if (i != properties.Length - 1)
                {
                    stringBuilder.Append(",");
                }

                stringBuilder.Append(" ");
            }

            stringBuilder.Append("FROM [");
            stringBuilder.Append(config.ModelConfiguration.TableName);
            stringBuilder.Append("]");

            return stringBuilder;
        }

        public string SelectById(ISqlResourceConfiguration config)
        {
            return SelectByIdBuilder(config).ToString();
        }

        public StringBuilder SelectByIdBuilder(ISqlResourceConfiguration config)
        {
            return SelectAllBuilder(config)
                .AppendWhereClause(config.PrimaryIdentifier.ColumnName, _sqlParamaterProvider.GetPrimaryKeyParameterName());
        }

        public string Insert(ISqlResourceConfiguration config)
        {
            ISqlPropertyConfiguration[] properties = config.PropertyConfigurationsForWrite.ToArray();
            StringBuilder stringBuilder = new StringBuilder("INSERT INTO [");
            stringBuilder.Append(config.ModelConfiguration.TableName);
            stringBuilder.Append("] (");

            for (int i = 0; i < properties.Length; i++)
            {
                stringBuilder.Append("[");
                stringBuilder.Append(properties[i].ColumnName);
                stringBuilder.Append("]");

                if (i != properties.Length - 1)
                {
                    stringBuilder.Append(", ");
                }
            }

            stringBuilder.Append(") VALUES (");

            for (int i = 0; i < properties.Length; i++)
            {
                stringBuilder.Append(_sqlParamaterProvider.GenerateParameterName(properties[i].PropertyName));

                if (i != properties.Length - 1)
                {
                    stringBuilder.Append(", ");
                }
            }

            stringBuilder.Append(") ");

            stringBuilder.Append(
                SelectAllBuilder(config)
                .AppendWhereClause(config.PrimaryIdentifier.ColumnName, "SCOPE_IDENTITY()"));

            return stringBuilder.ToString();
        }

        public string Update(ISqlResourceConfiguration config)
        {
            ISqlPropertyConfiguration[] properties = config.PropertyConfigurationsForWrite.ToArray();
            StringBuilder stringBuilder = new StringBuilder("UPDATE [");
            stringBuilder.Append(config.ModelConfiguration.TableName);
            stringBuilder.Append("] SET ");

            for (int i = 0; i < properties.Length; i++)
            {
                stringBuilder.Append("[");
                stringBuilder.Append(properties[i].ColumnName);
                stringBuilder.Append("] = ");
                stringBuilder.Append(_sqlParamaterProvider.GenerateParameterName(properties[i].PropertyName));

                if (i != properties.Length - 1)
                {
                    stringBuilder.Append(",");
                }

                stringBuilder.Append(" ");
            }

            return stringBuilder
                .AppendWhereClause(config.PrimaryIdentifier.ColumnName, _sqlParamaterProvider.GetPrimaryKeyParameterName())
                .Append(SelectByIdBuilder(config)).ToString();
        }
    }
}
