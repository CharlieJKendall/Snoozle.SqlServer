using Microsoft.Extensions.Options;
using Snoozle.SqlServer.Configuration;
using Snoozle.SqlServer.Internal.Wrappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snoozle.SqlServer.Internal
{
    public class SqlExecutor : ISqlExecutor
    {
        private readonly string _connectionString;
        private readonly ISqlClassProvider _sqlClassProvider;

        public SqlExecutor(IOptions<SnoozleSqlServerOptions> options, ISqlClassProvider sqlClassProvider)
        {
            _connectionString = options.Value.ConnectionString;
            _sqlClassProvider = sqlClassProvider;
        }

        public async Task<IEnumerable<T>> ExecuteSelectAllAsync<T>(string sql, Func<IDatabaseResultReader, T> mappingFunc)
            where T : class, IRestResource
        {
            try
            {
                using (IDatabaseConnection connection = _sqlClassProvider.CreateSqlConnection(_connectionString))
                using (IDatabaseCommand command = _sqlClassProvider.CreateSqlCommand(sql, connection))
                {
                    await connection.OpenAsync().ConfigureAwait(false);

                    using (IDatabaseResultReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        List<T> results = new List<T>();

                        while (await reader.ReadAsync().ConfigureAwait(false))
                        {
                            results.Add(mappingFunc(reader));
                        }

                        return results;
                    }
                }
            }
            catch (InvalidCastException ex)
            {
                throw InvalidConfigurationException.DataTypeMismatch(typeof(T).Name, ex);
            }
        }

        public async Task<T> ExecuteSelectByIdAsync<T>(
            string sql,
            Func<IDatabaseResultReader, T> mappingFunc,
            Func<object, IDatabaseCommandParameter> paramProvider,
            object primaryKey)
            where T : class, IRestResource
        {
            try
            {
                using (IDatabaseConnection connection = _sqlClassProvider.CreateSqlConnection(_connectionString))
                using (IDatabaseCommand command = _sqlClassProvider.CreateSqlCommand(sql, connection))
                {
                    command.AddParameter(paramProvider(primaryKey));

                    await connection.OpenAsync().ConfigureAwait(false);

                    using (IDatabaseResultReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        return await reader.ReadAsync().ConfigureAwait(false) ? mappingFunc(reader) : default;
                    }
                }
            }
            catch (InvalidCastException ex)
            {
                throw InvalidConfigurationException.DataTypeMismatch(typeof(T).Name, ex);
            }
        }

        public async Task<bool> ExecuteDeleteByIdAsync(
           string sql,
           Func<object, IDatabaseCommandParameter> paramProvider,
           object primaryKey)
        {
            using (IDatabaseConnection connection = _sqlClassProvider.CreateSqlConnection(_connectionString))
            using (IDatabaseCommand command = _sqlClassProvider.CreateSqlCommand(sql, connection))
            {
                command.AddParameter(paramProvider(primaryKey));

                await connection.OpenAsync().ConfigureAwait(false);

                int numberOfRowsDeleted = await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                return numberOfRowsDeleted != 0;
            }
        }

        public async Task<T> ExecuteInsertAsync<T>(
            string sql,
            Func<object, List<IDatabaseCommandParameter>> paramProvider,
            Func<IDatabaseResultReader, T> mappingFunc,
            T resourceToCreate)
            where T : class, IRestResource
        {
            try
            {
                using (IDatabaseConnection connection = _sqlClassProvider.CreateSqlConnection(_connectionString))
                using (IDatabaseCommand command = _sqlClassProvider.CreateSqlCommand(sql, connection))
                {
                    command.AddParameters(paramProvider(resourceToCreate));

                    await connection.OpenAsync().ConfigureAwait(false);

                    using (IDatabaseResultReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        return await reader.ReadAsync().ConfigureAwait(false) ? mappingFunc(reader) : default;
                    }
                }
            }
            catch (InvalidCastException ex)
            {
                throw InvalidConfigurationException.DataTypeMismatch(typeof(T).Name, ex);
            }
        }

        public async Task<T> ExecuteUpdateAsync<T>(
            string sql,
            Func<object, List<IDatabaseCommandParameter>> paramProvider,
            Func<object, IDatabaseCommandParameter> primaryKeyParamProvider,
            Func<IDatabaseResultReader, T> mappingFunc,
            T resourceToUpdate,
            object primaryKey)
            where T : class, IRestResource
        {
            try
            {
                using (IDatabaseConnection connection = _sqlClassProvider.CreateSqlConnection(_connectionString))
                using (IDatabaseCommand command = _sqlClassProvider.CreateSqlCommand(sql, connection))
                {
                    command.AddParameters(paramProvider(resourceToUpdate));
                    command.AddParameter(primaryKeyParamProvider(primaryKey));

                    await connection.OpenAsync().ConfigureAwait(false);

                    using (IDatabaseResultReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        return await reader.ReadAsync().ConfigureAwait(false) ? mappingFunc(reader) : default;
                    }
                }
            }
            catch (InvalidCastException ex)
            {
                throw InvalidConfigurationException.DataTypeMismatch(typeof(T).Name, ex);
            }
        }
    }
}
