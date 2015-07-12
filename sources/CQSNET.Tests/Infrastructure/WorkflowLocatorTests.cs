using CQSNET.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CQSNET.Tests.Infrastructure
{
    [TestClass]
    public class WorkflowLocatorTests
    {
        [TestMethod]
        public void ResolveWorkflow()
        {
            // Arrange
            IWorkflowLocator locator = new WorkflowLocator(x =>
            {
                if (x == typeof(WorkflowStub))
                    return new WorkflowStub();
                else
                    return null;
            });

            // Act
            IWorkflow query = locator.Resolve<WorkflowStub>();

            // Assert
            Assert.IsNotNull(query);
        }

        public class WorkflowStub : IWorkflow
        {
        }
    }
}