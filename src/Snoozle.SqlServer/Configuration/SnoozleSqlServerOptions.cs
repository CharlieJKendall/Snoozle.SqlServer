using Snoozle.Abstractions;
using Snoozle.Configuration;

namespace Snoozle.SqlServer.Configuration
{
    public class SnoozleSqlServerOptions : SnoozleOptions
    {
        public string ConnectionString { get; set; }
    }
}
