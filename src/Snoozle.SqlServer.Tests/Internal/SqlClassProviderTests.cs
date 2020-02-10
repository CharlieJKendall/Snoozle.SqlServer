using NSubstitute;
using NUnit.Framework;
using Snoozle.SqlServer.Internal;
using Snoozle.SqlServer.Internal.Wrappers;
using System;
using System.Data;

namespace Snoozle.SqlServer.Tests.Internal
{
    [TestFixture]
    public class SqlClassProviderTests
    {
        public ISqlClassProvider CreateSut()
        {
            return new SqlClassProvider();
        }

        #region CreateSqlCommand

        [Test]
        public void CreateSqlCommand_NullSqlParameter_ThrowsArgumentNullException()
        {
            // Arrange
            ISqlClassProvider sut = CreateSut();
            TestDelegate result = () => sut.CreateSqlCommand(null, Substitute.For<IDatabaseConnection>());

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(result);
        }

        [Test]
        public void CreateSqlCommand_NullSqlConnectionParameter_ThrowsArgumentNullException()
        {
            // Arrange
            ISqlClassProvider sut = CreateSut();
            string sql = "SELECT * FROM TABLE";
            TestDelegate result = () => sut.CreateSqlCommand(sql, null);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(result);
        }

        [Test]
        public void CreateSqlCommand_CorrectParameters_ReturnsIDatabaseCommandInstance()
        {
            // Arrange
            ISqlClassProvider sut = CreateSut();
            string sql = "SELECT * FROM TABLE";

            // Act
            var result = sut.CreateSqlCommand(sql, Substitute.For<IDatabaseConnection>());

            // Assert
            Assert.IsInstanceOf<IDatabaseCommand>(result);
        }

        #endregion

        #region CreateSqlConnection

        [Test]
        public void CreateSqlConnection_NullConnectionStringParameter_ThrowsArgumentNullException()
        {
            // Arrange
            ISqlClassProvider sut = CreateSut();
            TestDelegate result = () => sut.CreateSqlConnection(null);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(result);
        }

        [Test]
        public void CreateSqlConnection_CorrectParameters_ReturnsIDatabaseConnectionInstance()
        {
            // Arrange
            ISqlClassProvider sut = CreateSut();
            string connectionString = string.Empty;

            // Act
            var result = sut.CreateSqlConnection(connectionString);

            // Assert
            Assert.IsInstanceOf<IDatabaseConnection>(result);
        }

        #endregion

        #region CreateDatabaseCommandParameter

        [Test]
        public void CreateDatabaseCommandParameter_CreateDatabaseCommandParameter_ReturnsIDatabaseCommandParameterInstance()
        {
            // Arrange
            ISqlClassProvider sut = CreateSut();
            string paramName = string.Empty;
            object value = null;
            SqlDbType sqlDbType = SqlDbType.Int;
            bool isNullable = false;

            // Act
            var result = sut.CreateDatabaseCommandParameter(paramName, value, sqlDbType, isNullable);

            // Assert
            Assert.IsInstanceOf<IDatabaseCommandParameter>(result);
        }

        #endregion
    }
}
