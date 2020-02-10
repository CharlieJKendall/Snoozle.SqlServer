using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Snoozle.SqlServer.Internal.Wrappers
{
    public interface IDatabaseConnection : IDisposable
    {
        SqlConnection SqlConnection { get; }

        Task OpenAsync();
    }
}
