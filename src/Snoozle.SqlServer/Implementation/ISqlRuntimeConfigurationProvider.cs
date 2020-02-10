using Snoozle.Abstractions;

namespace Snoozle.SqlServer.Implementation
{
    public interface ISqlRuntimeConfigurationProvider : IRuntimeConfigurationProvider<ISqlRuntimeConfiguration<IRestResource>>
    {
    }
}
