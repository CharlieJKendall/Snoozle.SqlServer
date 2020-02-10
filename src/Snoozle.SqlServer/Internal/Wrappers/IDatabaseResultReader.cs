using System;
using System.Threading.Tasks;

namespace Snoozle.SqlServer.Internal.Wrappers
{
    public interface IDatabaseResultReader : IDisposable
    {
        Task<bool> ReadAsync();
        bool GetBoolean(int i);
        byte GetByte(int i);
        char GetChar(int i);
        DateTime GetDateTime(int i);
        DateTimeOffset GetDateTimeOffset(int i);
        decimal GetDecimal(int i);
        double GetDouble(int i);
        float GetFloat(int i);
        Guid GetGuid(int i);
        short GetInt16(int i);
        int GetInt32(int i);
        long GetInt64(int i);
        string GetString(int i);
        TimeSpan GetTimeSpan(int i);
    }
}
