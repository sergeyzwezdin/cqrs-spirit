using System.Reflection;
using CqrsSpirit.Tests.Stubs;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CqrsSpirit.Tests
{
    public class CqrsSpiritExtensionsTests
    {
        [Fact]
        public void CommandsDispatcherRegister()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.AddCqrsSpirit(new Assembly[] { });

            // Assert
            var dispatcher = serviceCollection.Find(s => s.ServiceType == typeof(ICommandsDispatcher));

            Assert.NotNull(dispatcher);
            Assert.Equal(ServiceLifetime.Singleton, dispatcher.Lifetime);
            Assert.Equal(typeof(CommandsDispatcher), dispatcher.ImplementationType);
        }

        [Fact]
        public void QueriesRegister()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.AddCqrsSpirit(new[] { typeof(IQuery1), typeof(Query1), typeof(IQuery2), typeof(Query2), typeof(IQuery3) });

            // Assert
            var query1 = serviceCollection.Find(s => s.ServiceType == typeof(IQuery1));
            Assert.NotNull(query1);
            Assert.Equal(ServiceLifetime.Transient, query1.Lifetime);
            Assert.Equal(typeof(Query1), query1.ImplementationType);

            var query2 = serviceCollection.Find(s => s.ServiceType == typeof(IQuery2));
            Assert.NotNull(query2);
            Assert.Equal(ServiceLifetime.Transient, query2.Lifetime);
            Assert.Equal(typeof(Query2), query2.ImplementationType);
        }

        [Fact]
        public void QueriesRegister_AmbiguousCheck()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            Assert.Throws<AmbiguousMatchException>(() => serviceCollection.AddCqrsSpirit(new[] { typeof(IQuery1), typeof(Query1), typeof(Query11), typeof(IQuery2), typeof(Query2), typeof(IQuery3) }));
        }

        [Fact]
        public void CommandsRegister()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.AddCqrsSpirit(new[] { typeof(Command1), typeof(Command1Handler), typeof(Command2), typeof(Command2Handler), typeof(Command3) });

            // Assert
            var handler1 = serviceCollection.Find(s => s.ServiceType == typeof(ICommandHandler<Command1>));
            Assert.NotNull(handler1);
            Assert.Equal(ServiceLifetime.Transient, handler1.Lifetime);
            Assert.Equal(typeof(Command1Handler), handler1.ImplementationType);

            var handler2 = serviceCollection.Find(s => s.ServiceType == typeof(ICommandHandler<Command2>));
            Assert.NotNull(handler2);
            Assert.Equal(ServiceLifetime.Transient, handler2.Lifetime);
            Assert.Equal(typeof(Command2Handler), handler2.ImplementationType);
        }

        [Fact]
        public void CommandsRegister_AmbiguousCheck()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            Assert.Throws<AmbiguousMatchException>(() => serviceCollection.AddCqrsSpirit(new[] { typeof(Command1), typeof(Command1Handler), typeof(Command11Handler), typeof(Command2), typeof(Command2Handler), typeof(Command3) }));
        }

        [Fact]
        public void WorkflowsRegister()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.AddCqrsSpirit(new[] { typeof(IWorkflow1), typeof(Workflow1), typeof(IWorkflow2), typeof(Workflow2), typeof(IWorkflow3) });

            // Assert
            var workflow1 = serviceCollection.Find(s => s.ServiceType == typeof(IWorkflow1));
            Assert.NotNull(workflow1);
            Assert.Equal(ServiceLifetime.Transient, workflow1.Lifetime);
            Assert.Equal(typeof(Workflow1), workflow1.ImplementationType);

            var workflow2 = serviceCollection.Find(s => s.ServiceType == typeof(IWorkflow2));
            Assert.NotNull(workflow2);
            Assert.Equal(ServiceLifetime.Transient, workflow2.Lifetime);
            Assert.Equal(typeof(Workflow2), workflow2.ImplementationType);
        }

        [Fact]
        public void WorkflowsRegister_AmbiguousCheck()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            Assert.Throws<AmbiguousMatchException>(() => serviceCollection.AddCqrsSpirit(new[] { typeof(IWorkflow1), typeof(Workflow1), typeof(Workflow11), typeof(IWorkflow2), typeof(Workflow2), typeof(IWorkflow3) }));
        }
    }
}