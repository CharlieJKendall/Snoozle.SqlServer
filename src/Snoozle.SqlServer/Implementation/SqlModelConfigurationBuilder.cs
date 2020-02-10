using Snoozle.Abstractions;

namespace Snoozle.SqlServer.Implementation
{
    public class SqlModelConfigurationBuilder : BaseModelConfigurationBuilder<ISqlModelConfiguration>, IModelConfigurationBuilder<ISqlModelConfiguration>
    {
        public SqlModelConfigurationBuilder(ISqlModelConfiguration modelConfiguration)
            : base(modelConfiguration)
        {
        }
    }
}