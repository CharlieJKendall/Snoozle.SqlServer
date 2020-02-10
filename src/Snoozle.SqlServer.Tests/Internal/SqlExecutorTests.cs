using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using Snoozle.SqlServer.Configuration;
using Snoozle.SqlServer.Internal;
using Snoozle.SqlServer.Internal.Wrappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snoozle.SqlServer.Tests.Internal
{
    [TestFixture]
    public class SqlExecutorTests
    {
        public ISqlExecutor CreateSut(ISqlClassProvider sqlClassProvider = null)
        {
            var optionsMock = Substitute.For<IOptions<SnoozleSqlServerOptions>>();
            optionsMock.Value.Returns(new SnoozleSqlServerOptions());

            return new SqlExecutor(optionsMock, sqlClassProvider ?? Substitute.For<ISqlClassProvider>());
        }

        #region ExecuteSelectAllAsync

        [Test]
        public async Task ExecuteSelectAllAsync_AcceptablParametersProvided_ReturnsNonNullObject()
        {
            // Arrange
            var sut = CreateSut();
            Func<IDatabaseResultReader, IRestResource> mappingFunc = (data) => null;

            // Act
            var result = await sut.ExecuteSelectAllAsync(string.Empty, mappingFunc);

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task ExecuteSelectAllAsync_AcceptablParametersProvided_ReturnsInstancesOfResource()
        {
            // Arrange
            var sut = CreateSut();
            Func<IDatabaseResultReader, IRestResource> mappingFunc = (data) => null;

            // Act
            var result = await sut.ExecuteSelectAllAsync(string.Empty, mappingFunc);

            // Assert
            Assert.IsInstanceOf<IEnumerable<IRestResource>>(result);
        }

        #endregion
    }
}
