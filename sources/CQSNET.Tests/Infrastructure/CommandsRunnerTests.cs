using CQSNET.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;

namespace CQSNET.Tests.Infrastructure
{
	[TestClass]
	public class CommandsRunnerTests
	{
		[TestMethod]
		public void ExecuteCommandWithSingleHandler()
		{
			// Arrange
			Mock<ICommandHandler<CommandStub>> handlerMock = new Mock<ICommandHandler<CommandStub>>();
			ICommandsRunner runner = new CommandsRunner(x =>
			{
				if (typeof(ICommandHandler<CommandStub>).IsAssignableFrom(x))
					return new[] { handlerMock.Object };
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
		public void ExecuteCommandWithMultiplieHandler()
		{
			// Arrange
			Mock<ICommandHandler<CommandStub>> handler1Mock = new Mock<ICommandHandler<CommandStub>>();
			Mock<ICommandHandler<CommandStub>> handler2Mock = new Mock<ICommandHandler<CommandStub>>();
			ICommandsRunner runner = new CommandsRunner(x =>
			{
				if (typeof(ICommandHandler<CommandStub>).IsAssignableFrom(x))
					return new[] { handler1Mock.Object, handler2Mock.Object };
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

		public class CommandStub : ICommand
		{
		}
	}
}
