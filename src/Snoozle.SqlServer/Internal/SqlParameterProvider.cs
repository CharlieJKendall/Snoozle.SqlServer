using Snoozle.Exceptions;

namespace Snoozle.SqlServer.Internal
{
    public class SqlParameterProvider : ISqlParamaterProvider
    {
        private const string ID_PARAM_NAME = "PrimaryKeyIdParam";

        public string GenerateParameterName(string propertyName)
        {
            ExceptionHelper.Argument.ThrowIfTrue(
                propertyName == ID_PARAM_NAME,
                $"Property cannot be called '{ID_PARAM_NAME}'; this is reserved for internal usage.",
                nameof(propertyName));

            return $"@{propertyName}";
        }

        public string GetPrimaryKeyParameterName()
        {
            return $"@{ID_PARAM_NAME}";
        }
    }
}
