using CQSNET.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CQSNET.Tests.Infrastructure
{
    [TestClass]
    public class CommandsRunnerTests
    {
        [TestMethod]
        public void ExecuteCommandWithSingleHandlerSync()
        {
            // Arrange
            Mock<ICommandHandler<CommandStub>> handlerMock = new Mock<ICommandHandler<CommandStub>>();
            ICommandsRunner runner = new CommandsRunner(x =>
            {
                if (typeof(ICommandHandler<CommandStub>).IsAssignableFrom(x))
                    return new[] { handlerMock.Object };
                else if (typeof(ICommandAsyncHandler<CommandStub>).IsAssignableFrom(x))
                    return new ICommandAsyncHandler<CommandStub>[] { };
                else
                    return null;
            });

            // Act
            CommandStub command = new CommandStub();
            runner.Execute(command);

            // Assert
            handlerMock.Verify(x => x.Execute(It.Is<CommandStub>(p => p == command)), Times.Once);
        }

        [TestMethod]
        public void ExecuteCommandWithMultiplieHandlerSync()
        {
            // Arrange
            Mock<ICommandHandler<CommandStub>> handler1Mock = new Mock<ICommandHandler<CommandStub>>();
            Mock<ICommandHandler<CommandStub>> handler2Mock = new Mock<ICommandHandler<CommandStub>>();
            ICommandsRunner runner = new CommandsRunner(x =>
            {
                if (typeof(ICommandHandler<CommandStub>).IsAssignableFrom(x))
                    return new[] { handler1Mock.Object, handler2Mock.Object };
                else if (typeof(ICommandAsyncHandler<CommandStub>).IsAssignableFrom(x))
                    return new ICommandAsyncHandler<CommandStub>[] { };
                else
                    return null;
            });

            // Act
            CommandStub command = new CommandStub();
            runner.Execute(command);

            // Assert
            handler1Mock.Verify(x => x.Execute(It.Is<CommandStub>(p => p == command)), Times.Once);
            handler2Mock.Verify(x => x.Execute(It.Is<CommandStub>(p => p == command)), Times.Once);
        }

        [TestMethod]
        public void ExecuteCommandWithMultiplieHandlerAsync()
        {
            // Arrange
            Mock<ICommandHandler<CommandStub>> handler1Mock = new Mock<ICommandHandler<CommandStub>>();
            Mock<ICommandAsyncHandler<CommandStub>> handler2Mock = new Mock<ICommandAsyncHandler<CommandStub>>();
            ICommandsRunner runner = new CommandsRunner(x =>
            {
                if (typeof(ICommandHandler<CommandStub>).IsAssignableFrom(x))
                    return new[] { handler1Mock.Object };
                else if (typeof(ICommandAsyncHandler<CommandStub>).IsAssignableFrom(x))
                    return new ICommandAsyncHandler<CommandStub>[] { handler2Mock.Object };
                else
                    return null;
            });

            // Act
            CommandStub command = new CommandStub();
            runner.Execute(command);

            // Assert
            handler1Mock.Verify(x => x.Execute(It.Is<CommandStub>(p => p == command)), Times.Once);
            handler2Mock.Verify(x => x.ExecuteAsync(It.Is<CommandStub>(p => p == command)), Times.Once);
        }

        public class CommandStub : ICommand
        {
        }
    }
}
