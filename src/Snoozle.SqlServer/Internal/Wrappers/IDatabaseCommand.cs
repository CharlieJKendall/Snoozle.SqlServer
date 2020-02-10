using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snoozle.SqlServer.Internal.Wrappers
{
    public interface IDatabaseCommand : IDisposable
    {
        Task<IDatabaseResultReader> ExecuteReaderAsync();
        Task<int> ExecuteNonQueryAsync();
        void AddParameter(IDatabaseCommandParameter databaseCommandParameter);
        void AddParameters(IEnumerable<IDatabaseCommandParameter> databaseCommandParameters);
    }
}
