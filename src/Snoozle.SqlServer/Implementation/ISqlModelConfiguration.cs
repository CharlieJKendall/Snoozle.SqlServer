using Snoozle.Abstractions;

namespace Snoozle.SqlServer.Implementation
{
    public interface ISqlModelConfiguration : IModelConfiguration
    {
        string TableName { get; set; }
    }
}