using System.Data.SqlClient;

namespace Snoozle.SqlServer.Internal.Wrappers
{
    public interface IDatabaseCommandParameter
    {
        SqlParameter SqlParameter { get; }
    }
}
