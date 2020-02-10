using Snoozle.Abstractions;
using Snoozle.SqlServer.Extensions;
using Snoozle.SqlServer.Implementation;
using System;
using System.Collections.Generic;
using System.Data;

namespace Snoozle.SqlServer
{
    public abstract class SqlResourceConfigurationBuilder<TResource> : BaseResourceConfigurationBuilder<TResource, ISqlPropertyConfiguration, ISqlResourceConfiguration, ISqlModelConfiguration>
        where TResource : class, IRestResource
    {
        private readonly Dictionary<Type, SqlDbType?> _defaultSqlDbTypeMap = new Dictionary<Type, SqlDbType?>
        {
            { typeof(long), SqlDbType.BigInt },
            { typeof(byte[]), SqlDbType.VarBinary },
            { typeof(bool), SqlDbType.Bit },
            { typeof(string), SqlDbType.NVarChar },
            { typeof(DateTime), SqlDbType.DateTime },
            { typeof(DateTimeOffset), SqlDbType.DateTimeOffset },
            { typeof(decimal), SqlDbType.Decimal },
            { typeof(double), SqlDbType.Float },
            { typeof(float), SqlDbType.Float },
            { typeof(int), SqlDbType.Int },
            { typeof(short), SqlDbType.SmallInt },
            { typeof(TimeSpan), SqlDbType.Time },
            { typeof(Guid), SqlDbType.UniqueIdentifier },
            { typeof(byte), SqlDbType.TinyInt },
            { typeof(char), SqlDbType.Char }
        };

        protected override IPropertyConfigurationBuilder<TProperty, ISqlPropertyConfiguration> CreatePropertyConfigurationBuilder<TProperty>(
            ISqlPropertyConfiguration propertyConfiguration)
        {
            return new SqlPropertyConfigurationBuilder<TResource, TProperty>(propertyConfiguration);
        }

        protected override ISqlResourceConfiguration CreateResourceConfiguration()
        {
            return new SqlResourceConfiguration<TResource>(ModelConfiguration, PropertyConfigurations);
        }

        protected override ISqlModelConfiguration CreateModelConfiguration()
        {
            return new SqlModelConfiguration<TResource>();
        }

        protected override ISqlPropertyConfiguration CreatePropertyConfiguration()
        {
            return new SqlPropertyConfiguration();
        }

        protected override IModelConfigurationBuilder<ISqlModelConfiguration> CreateModelConfigurationBuilder()
        {
            return new SqlModelConfigurationBuilder(ModelConfiguration);
        }

        protected override void SetPropertyConfigurationDefaults()
        {
            base.SetPropertyConfigurationDefaults();

            foreach (ISqlPropertyConfiguration propertyConfig in PropertyConfigurations)
            {
                propertyConfig.SqlDbType = GetDefaultSqlDbType(propertyConfig.PropertyType)
                    ?? throw new InvalidOperationException($"{propertyConfig.PropertyType.Name} is not a valid property type.");
                propertyConfig.ColumnName = propertyConfig.PropertyName;
            }
        }

        private SqlDbType? GetDefaultSqlDbType(Type type)
        {
            type.TryUnwrapNullableType(out Type unwrappedType);

            return unwrappedType.IsEnum ? SqlDbType.Int : _defaultSqlDbTypeMap.GetValueOrDefault(unwrappedType);
        }
    }
}
