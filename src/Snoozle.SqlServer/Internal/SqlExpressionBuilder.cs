using Snoozle.SqlServer.Extensions;
using Snoozle.SqlServer.Implementation;
using Snoozle.SqlServer.Internal.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Snoozle.SqlServer.Internal
{
    public class SqlExpressionBuilder : ISqlExpressionBuilder
    {
        private readonly ISqlParamaterProvider _sqlParamaterProvider;
        private readonly ISqlClassProvider _sqlClassProvider;

        public SqlExpressionBuilder(ISqlParamaterProvider sqlParamaterProvider, ISqlClassProvider sqlClassProvider)
        {
            _sqlParamaterProvider = sqlParamaterProvider;
            _sqlClassProvider = sqlClassProvider;
        }

        public Func<object, IDatabaseCommandParameter> GetPrimaryKeySqlParameter(ISqlPropertyConfiguration primaryIdentifierConfig)
        {
            return GetSqlParameter(primaryIdentifierConfig, _sqlParamaterProvider.GetPrimaryKeyParameterName(), false).Compile();
        }

        public Func<object, List<IDatabaseCommandParameter>> GetNonPrimaryKeySqlParameters(ISqlResourceConfiguration config)
        {
            ISqlPropertyConfiguration[] configs = config.PropertyConfigurationsForWrite.ToArray();

            var paramResource = Expression.Parameter(typeof(object), "resourceObject");
            var typedParam = Expression.Convert(paramResource, config.ResourceType);
            var result = Expression.Variable(typeof(List<IDatabaseCommandParameter>), "databaseCommandParameter");
            var typedParamVar = Expression.Variable(config.ResourceType, "typedParam");
            var assignTypedParamVar = Expression.Assign(typedParamVar, typedParam);
            var instantiateResultObj = Expression.Assign(result, Expression.New(typeof(List<IDatabaseCommandParameter>)));

            List<Expression> expressions = new List<Expression>
            {
                assignTypedParamVar,
                instantiateResultObj
            };

            for (int i = 0; i < configs.Length; i++)
            {
                var property = Expression.Property(typedParamVar, configs[i].PropertyName);
                var sqlParam = GetSqlParameter(configs[i], _sqlParamaterProvider.GenerateParameterName(configs[i].PropertyName));
                var getSqlParam = Expression.Invoke(sqlParam, Expression.Convert(property, typeof(object)));
                var generateNameAndAddToList = Expression.Call(
                    result,
                    nameof(List<IDatabaseCommandParameter>.Add),
                    null,
                    getSqlParam);

                expressions.Add(generateNameAndAddToList);
            }

            // Return the result from the expression
            expressions.Add(result);

            var lambda = Expression.Lambda<Func<object, List<IDatabaseCommandParameter>>>(
                Expression.Block(
                    new[] { result, typedParamVar },
                    expressions),
                paramResource);

            return lambda.Compile();
        }

        private Expression<Func<object, IDatabaseCommandParameter>> GetSqlParameter(ISqlPropertyConfiguration config, string paramName, bool isNullable = true)
        {
            return (value) => _sqlClassProvider.CreateDatabaseCommandParameter(paramName, value, config.SqlDbType.Value, isNullable);               
        }

        public Func<IDatabaseResultReader, T> CreateObjectRelationalMap<T>(ISqlResourceConfiguration config)
            where T : class, IRestResource
        {
            ISqlPropertyConfiguration[] orderedConfigs = config.PropertyConfigurationsForRead.OrderBy(prop => prop.Index).ToArray();

            var paramDataReader = Expression.Parameter(typeof(IDatabaseResultReader), "databaseResultReader");
            var result = Expression.Variable(typeof(T), "result");
            var instantiateResultObj = Expression.Assign(result, Expression.New(typeof(T)));

            List<Expression> expressions = new List<Expression>
            {
                instantiateResultObj
            };

            // Add assignment expression for each property that uses correct read method on SqlDataReader for the property type
            for (int i = 0; i < orderedConfigs.Length; i++)
            {
                var property = Expression.Property(result, orderedConfigs[i].PropertyName);
                var assignProperty = Expression.Assign(
                    Expression.MakeMemberAccess(result, property.Member),
                    GetMethodCallForType(paramDataReader, Expression.Constant(i), property));

                expressions.Add(assignProperty);
            }

            // Return the result from the expression
            expressions.Add(result);

            var lambda = Expression.Lambda<Func<IDatabaseResultReader, T>>(
                Expression.Block(
                    new[] { result },
                    expressions),
                paramDataReader);

            return lambda.Compile();
        }

        private Expression GetMethodCallForType(Expression dataReaderInstance, Expression dataIndex, MemberExpression property)
        {
            bool wasUnwrapped = property.Type.TryUnwrapNullableType(out Type unwrappedTypeOrOriginal);

            switch (unwrappedTypeOrOriginal)
            {
                case Type _ when unwrappedTypeOrOriginal == typeof(string):
                    return GetMethodCallWithCast(wasUnwrapped, dataReaderInstance, nameof(IDatabaseResultReader.GetString), dataIndex, property);
                case Type _ when unwrappedTypeOrOriginal == typeof(int):
                    return GetMethodCallWithCast(wasUnwrapped, dataReaderInstance, nameof(IDatabaseResultReader.GetInt32), dataIndex, property);
                case Type _ when unwrappedTypeOrOriginal == typeof(DateTime):
                    return GetMethodCallWithCast(wasUnwrapped, dataReaderInstance, nameof(IDatabaseResultReader.GetDateTime), dataIndex, property);
                case Type _ when unwrappedTypeOrOriginal == typeof(double):
                    return GetMethodCallWithCast(wasUnwrapped, dataReaderInstance, nameof(IDatabaseResultReader.GetDouble), dataIndex, property);
                case Type _ when unwrappedTypeOrOriginal == typeof(float):
                    return GetMethodCallWithCast(wasUnwrapped, dataReaderInstance, nameof(IDatabaseResultReader.GetFloat), dataIndex, property);
                case Type _ when unwrappedTypeOrOriginal == typeof(long):
                    return GetMethodCallWithCast(wasUnwrapped, dataReaderInstance, nameof(IDatabaseResultReader.GetInt64), dataIndex, property);
                case Type _ when unwrappedTypeOrOriginal == typeof(DateTimeOffset):
                    return GetMethodCallWithCast(wasUnwrapped, dataReaderInstance, nameof(IDatabaseResultReader.GetDateTimeOffset), dataIndex, property);
                case Type _ when unwrappedTypeOrOriginal == typeof(decimal):
                    return GetMethodCallWithCast(wasUnwrapped, dataReaderInstance, nameof(IDatabaseResultReader.GetDecimal), dataIndex, property);
                case Type _ when unwrappedTypeOrOriginal == typeof(char):
                    return GetMethodCallWithCast(wasUnwrapped, dataReaderInstance, nameof(IDatabaseResultReader.GetChar), dataIndex, property);
                case Type _ when unwrappedTypeOrOriginal == typeof(TimeSpan):
                    return GetMethodCallWithCast(wasUnwrapped, dataReaderInstance, nameof(IDatabaseResultReader.GetTimeSpan), dataIndex, property);
                case Type _ when unwrappedTypeOrOriginal == typeof(byte):
                    return GetMethodCallWithCast(wasUnwrapped, dataReaderInstance, nameof(IDatabaseResultReader.GetByte), dataIndex, property);
                case Type _ when unwrappedTypeOrOriginal == typeof(short):
                    return GetMethodCallWithCast(wasUnwrapped, dataReaderInstance, nameof(IDatabaseResultReader.GetInt16), dataIndex, property);
                case Type _ when unwrappedTypeOrOriginal == typeof(Guid):
                    return GetMethodCallWithCast(wasUnwrapped, dataReaderInstance, nameof(IDatabaseResultReader.GetGuid), dataIndex, property);
                case Type _ when unwrappedTypeOrOriginal.IsEnum:
                    return GetMethodCallWithCast(true, dataReaderInstance, nameof(IDatabaseResultReader.GetInt32), dataIndex, property);
                default:
                    throw new NotSupportedException($"Type {unwrappedTypeOrOriginal.Name} is not supported.");
            }
        }

        private Expression GetMethodCallWithCast(bool shouldCastToPropertyType, Expression dataReaderInstance, string dataReaderMethodName, Expression dataIndex, MemberExpression property)
        {
            if (shouldCastToPropertyType)
            {
                return Expression.Convert(Expression.Call(dataReaderInstance, dataReaderMethodName, null, dataIndex), property.Type);
            }
            else
            {
                return Expression.Call(dataReaderInstance, dataReaderMethodName, null, dataIndex);
            }
        }
    }

}
