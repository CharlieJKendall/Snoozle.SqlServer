using Snoozle.Abstractions;
using Snoozle.SqlServer.Internal.Wrappers;
using System;
using System.Collections.Generic;

namespace Snoozle.SqlServer.Implementation
{
    internal class SqlRuntimeConfiguration<TResource> : BaseRuntimeConfiguration<ISqlPropertyConfiguration, ISqlModelConfiguration, TResource>, ISqlRuntimeConfiguration<TResource>
        where TResource : class, IRestResource
    {
        // This is created by reflection, so be careful when changing/adding parameters
        public SqlRuntimeConfiguration(
            ISqlResourceConfiguration resourceConfiguration,
            Func<IDatabaseResultReader, TResource> getSqlMapToResource,
            Func<object, IDatabaseCommandParameter> getPrimaryKeySqlParameter,
            Func<object, List<IDatabaseCommandParameter>> getNonPrimaryKeySqlParameters,
            string selectAll,
            string selectById,
            string deleteById,
            string insert,
            string updateById)
            : base(resourceConfiguration)
        {
            GetSqlMapToResource = getSqlMapToResource;
            GetPrimaryKeySqlParameter = getPrimaryKeySqlParameter;
            GetNonPrimaryKeySqlParameters = getNonPrimaryKeySqlParameters;
            SelectAll = selectAll;
            SelectById = selectById;
            DeleteById = deleteById;
            Insert = insert;
            UpdateById = updateById;
        }

        public Func<IDatabaseResultReader, TResource> GetSqlMapToResource { get; }
        public Func<object, IDatabaseCommandParameter> GetPrimaryKeySqlParameter { get; }
        public Func<object, List<IDatabaseCommandParameter>> GetNonPrimaryKeySqlParameters { get; }
        public string SelectAll { get; }
        public string SelectById { get; }
        public string DeleteById { get; }
        public string Insert { get; }
        public string UpdateById { get; }
    }
}
