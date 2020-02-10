using Snoozle.SqlServer.Internal.Wrappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snoozle.SqlServer.Internal
{
    public interface ISqlExecutor
    {
        Task<T> ExecuteSelectByIdAsync<T>(
            string sql,
            Func<IDatabaseResultReader, T> mappingFunc,
            Func<object, IDatabaseCommandParameter> paramProvider,
            object primaryKey) where T : class, IRestResource;

        Task<IEnumerable<T>> ExecuteSelectAllAsync<T>(
            string sql,
            Func<IDatabaseResultReader, T> mappingFunc) where T : class, IRestResource;

        Task<bool> ExecuteDeleteByIdAsync(
           string sql,
           Func<object, IDatabaseCommandParameter> paramProvider,
           object primaryKey);

        Task<T> ExecuteInsertAsync<T>(
            string sql,
            Func<object, List<IDatabaseCommandParameter>> paramProvider,
            Func<IDatabaseResultReader, T> mappingFunc,
            T resource) where T : class, IRestResource;

        Task<T> ExecuteUpdateAsync<T>(
            string sql,
            Func<object, List<IDatabaseCommandParameter>> paramProvider,
            Func<object, IDatabaseCommandParameter> primaryKeyParamProvider,
            Func<IDatabaseResultReader, T> mappingFunc,
            T resourceToCreate,
            object primaryKey) where T : class, IRestResource;
    }
}