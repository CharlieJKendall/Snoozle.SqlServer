namespace Snoozle.SqlServer.Internal
{
    public interface ISqlParamaterProvider
    {
        string GenerateParameterName(string propertyName);
        string GetPrimaryKeyParameterName();
    }
}
