using Snoozle.Abstractions;

namespace Snoozle.SqlServer.Implementation
{
    public class SqlModelConfiguration<TResource> : BaseModelConfiguration<TResource>, ISqlModelConfiguration
        where TResource : class, IRestResource
    {
        public string TableName { get; set; } = typeof(TResource).Name;
    }
}
