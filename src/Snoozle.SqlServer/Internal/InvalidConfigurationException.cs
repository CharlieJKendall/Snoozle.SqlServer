using System;

namespace Snoozle.SqlServer.Internal
{
    public class InvalidConfigurationException : Exception
    {
        public InvalidConfigurationException()
        {
        }

        public InvalidConfigurationException(string message)
            : base(message)
        {
        }

        public InvalidConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public static InvalidConfigurationException DataTypeMismatch(string typeName, Exception innerException = null)
        {
            return new InvalidConfigurationException(
                $"Configuration mismatch between database schema and model property types. Ensure all properties on the {typeName} resource have the correct types and are mapped to the correct database columns.",
                innerException);
        }
    }
}
