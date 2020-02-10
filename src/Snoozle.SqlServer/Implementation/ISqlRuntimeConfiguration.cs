using Snoozle.Abstractions;
using Snoozle.SqlServer.Internal.Wrappers;
using System;
using System.Collections.Generic;

namespace Snoozle.SqlServer.Implementation
{
    public interface ISqlRuntimeConfiguration<out TResource> : IRuntimeConfiguration
        where TResource : class, IRestResource
    {
        Func<IDatabaseResultReader, TResource> GetSqlMapToResource { get; }
        Func<object, IDatabaseCommandParameter> GetPrimaryKeySqlParameter { get; }
        Func<object, List<IDatabaseCommandParameter>> GetNonPrimaryKeySqlParameters { get; }

        string SelectAll { get; }
        string SelectById { get; }
        string DeleteById { get; }
        string Insert { get; }
        string UpdateById { get; }
    }
}