using Microsoft.Extensions.Logging;
using Snoozle.Abstractions;
using Snoozle.SqlServer.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snoozle.SqlServer.Implementation
{
    internal class SqlDataProvider : IDataProvider
    {
        private readonly ISqlRuntimeConfigurationProvider _sqlRuntimeConfigurationProvider;
        private readonly ISqlExecutor _sqlExecutor;
        private readonly ILogger<SqlDataProvider> _logger;

        public SqlDataProvider(ISqlRuntimeConfigurationProvider sqlRuntimeConfigurationProvider, ISqlExecutor sqlExecutor, ILogger<SqlDataProvider> logger)
        {
            _sqlRuntimeConfigurationProvider = sqlRuntimeConfigurationProvider;
            _sqlExecutor = sqlExecutor;
            _logger = logger;
        }

        public Task<bool> DeleteByIdAsync<TResource>(object primaryKey)
            where TResource : class, IRestResource
        {
            var config = GetConfig<TResource>();

            return _sqlExecutor.ExecuteDeleteByIdAsync(
                config.DeleteById,
                config.GetPrimaryKeySqlParameter,
                primaryKey);
        }

        public Task<TResource> InsertAsync<TResource>(TResource resourceToCreate)
            where TResource : class, IRestResource
        {
            var config = GetConfig<TResource>();

            return _sqlExecutor.ExecuteInsertAsync(
                config.Insert,
                config.GetSqlParametersForCreation,
                config.GetSqlMapToResource,
                resourceToCreate);
        }

        public Task<IEnumerable<TResource>> SelectAllAsync<TResource>()
            where TResource : class, IRestResource
        {
            var config = GetConfig<TResource>();

            return _sqlExecutor.ExecuteSelectAllAsync(config.SelectAll, config.GetSqlMapToResource);
        }

        public Task<TResource> SelectByIdAsync<TResource>(object primaryKey)
            where TResource : class, IRestResource
        {
            var config = GetConfig<TResource>();

            try
            {
                return _sqlExecutor.ExecuteSelectByIdAsync(
                    config.SelectById,
                    config.GetSqlMapToResource,
                    config.GetPrimaryKeySqlParameter,
                    primaryKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to find resource of type {Type} with a primary key value of {PrimaryKey}", typeof(TResource).Name, primaryKey);

                return null;
            }
        }

        public Task<TResource> UpdateAsync<TResource>(TResource resourceToUpdate, object primaryKey)
            where TResource : class, IRestResource
        {
            var config = GetConfig<TResource>();

            return _sqlExecutor.ExecuteUpdateAsync(
                config.UpdateById,
                config.GetSqlParametersForUpdating,
                config.GetPrimaryKeySqlParameter,
                config.GetSqlMapToResource,
                resourceToUpdate,
                primaryKey);
        }

        private ISqlRuntimeConfiguration<TResource> GetConfig<TResource>()
            where TResource : class, IRestResource
        {
            return (ISqlRuntimeConfiguration<TResource>)_sqlRuntimeConfigurationProvider.GetRuntimeConfigurationForType(typeof(TResource));
        }
    }
}
