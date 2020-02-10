using Snoozle.SqlServer.Implementation;
using Snoozle.SqlServer.Internal.Wrappers;
using System;
using System.Collections.Generic;

namespace Snoozle.SqlServer.Internal
{
    public interface ISqlExpressionBuilder
    {
        Func<object, IDatabaseCommandParameter> GetPrimaryKeySqlParameter(ISqlPropertyConfiguration primaryIdentifierConfig);

        Func<IDatabaseResultReader, T> CreateObjectRelationalMap<T>(ISqlResourceConfiguration config)
            where T : class, IRestResource;

        Func<object, List<IDatabaseCommandParameter>> GetNonPrimaryKeySqlParameters(ISqlResourceConfiguration config);
    }
}
