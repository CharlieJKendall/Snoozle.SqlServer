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
            using (IDatabaseConnection connection = _sqlClassProvider.CreateSqlConnection(_connectionString))
            using (IDatabaseCommand command = _sqlClassProvider.CreateSqlCommand(sql, connection))
            {
                await connection.OpenAsync();

                using (IDatabaseResultReader reader = await command.ExecuteReaderAsync())
                {
                    List<T> results = new List<T>();

                    while (await reader.ReadAsync())
                    {
                        results.Add(mappingFunc(reader));
                    }

                    return results;
                }
            }
        }

        public async Task<T> ExecuteSelectByIdAsync<T>(
            string sql,
            Func<IDatabaseResultReader, T> mappingFunc,
            Func<object, IDatabaseCommandParameter> paramProvider,
            object primaryKey)
            where T : class, IRestResource
        {
            using (IDatabaseConnection connection = _sqlClassProvider.CreateSqlConnection(_connectionString))
            using (IDatabaseCommand command = _sqlClassProvider.CreateSqlCommand(sql, connection))
            {
                command.AddParameter(paramProvider(primaryKey));

                await connection.OpenAsync();

                using (IDatabaseResultReader reader = await command.ExecuteReaderAsync())
                {
                    return await reader.ReadAsync() ? mappingFunc(reader) : default;
                }
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

                await connection.OpenAsync();

                int numberOfRowsDeleted = await command.ExecuteNonQueryAsync();

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
            using (IDatabaseConnection connection = _sqlClassProvider.CreateSqlConnection(_connectionString))
            using (IDatabaseCommand command = _sqlClassProvider.CreateSqlCommand(sql, connection))
            {
                command.AddParameters(paramProvider(resourceToCreate));

                await connection.OpenAsync();

                using (IDatabaseResultReader reader = await command.ExecuteReaderAsync())
                {
                    return await reader.ReadAsync() ? mappingFunc(reader) : default;
                }
            }
        }

        public async Task<T> ExecuteUpdateAsync<T>(
            string sql,
            Func<object, List<IDatabaseCommandParameter>> paramProvider,
            Func<object, IDatabaseCommandParameter> primaryKeyParamProvider,
            Func<IDatabaseResultReader, T> mappingFunc,
            T resourceToCreate,
            object primaryKey)
            where T : class, IRestResource
        {
            using (IDatabaseConnection connection = _sqlClassProvider.CreateSqlConnection(_connectionString))
            using (IDatabaseCommand command = _sqlClassProvider.CreateSqlCommand(sql, connection))
            {
                command.AddParameters(paramProvider(resourceToCreate));
                command.AddParameter(primaryKeyParamProvider(primaryKey));

                await connection.OpenAsync();

                using (IDatabaseResultReader reader = await command.ExecuteReaderAsync())
                {
                    return await reader.ReadAsync() ? mappingFunc(reader) : default;
                }
            }
        }
    }
}
