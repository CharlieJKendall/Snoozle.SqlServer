using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Snoozle.SqlServer.Internal.Wrappers
{
    public class DatabaseResultReader : IDatabaseResultReader
    {
        public SqlDataReader SqlDataReader { get; }

        public DatabaseResultReader(SqlDataReader sqlDataReader)
        {
            SqlDataReader = sqlDataReader;
        }

        public Task<bool> ReadAsync()
        {
            return SqlDataReader.ReadAsync();
        }

        private T GetValueOrDefault<T>(Func<int, T> readerFunc, int index)
        {
            return SqlDataReader.IsDBNull(index) ? default : readerFunc(index);
        }

        public bool GetBoolean(int i)
        {
            return GetValueOrDefault((index) => SqlDataReader.GetBoolean(index), i);
        }

        public byte GetByte(int i)
        {
            return GetValueOrDefault((index) => SqlDataReader.GetByte(index), i);
        }

        public char GetChar(int i)
        {
            return GetValueOrDefault((index) => SqlDataReader.GetChar(index), i);
        }

        public DateTime GetDateTime(int i)
        {
            return GetValueOrDefault((index) => SqlDataReader.GetDateTime(index), i);
        }

        public DateTimeOffset GetDateTimeOffset(int i)
        {
            return GetValueOrDefault((index) => SqlDataReader.GetDateTimeOffset(index), i);
        }

        public decimal GetDecimal(int i)
        {
            return GetValueOrDefault((index) => SqlDataReader.GetDecimal(index), i);
        }

        public double GetDouble(int i)
        {
            return GetValueOrDefault((index) => SqlDataReader.GetDouble(index), i);
        }

        public float GetFloat(int i)
        {
            return GetValueOrDefault((index) => SqlDataReader.GetFloat(index), i);
        }

        public Guid GetGuid(int i)
        {
            return GetValueOrDefault((index) => SqlDataReader.GetGuid(index), i);
        }

        public short GetInt16(int i)
        {
            return GetValueOrDefault((index) => SqlDataReader.GetInt16(index), i);
        }

        public int GetInt32(int i)
        {
            return GetValueOrDefault((index) => SqlDataReader.GetInt32(index), i);
        }

        public long GetInt64(int i)
        {
            return GetValueOrDefault((index) => SqlDataReader.GetInt64(index), i);
        }

        public string GetString(int i)
        {
            return GetValueOrDefault((index) => SqlDataReader.GetString(index), i);
        }

        public TimeSpan GetTimeSpan(int i)
        {
            return GetValueOrDefault((index) => SqlDataReader.GetTimeSpan(index), i);
        }

        public void Dispose()
        {
            SqlDataReader?.Dispose();
        }
    }
}
