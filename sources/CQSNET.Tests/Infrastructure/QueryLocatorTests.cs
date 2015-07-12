using CQSNET.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CQSNET.Tests.Infrastructure
{
    [TestClass]
    public class QueryLocatorTests
    {
        [TestMethod]
        public void ResoveQuery()
        {
            // Arrange
            IQueryLocator locator = new QueryLocator(x =>
            {
                if (x == typeof(QueryStub))
                    return new QueryStub();
                else
                    return null;
            });

            // Act
            IQuery query = locator.Resolve<QueryStub>();

            // Assert
            Assert.IsNotNull(query);
        }

        public class QueryStub : IQuery
        {
        }
    }
}