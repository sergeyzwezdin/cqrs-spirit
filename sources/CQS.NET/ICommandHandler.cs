using System;

namespace CQS.NET
{
	public interface ICommandHandler<in TCommand> : IDisposable
		where TCommand : ICommand
	{
		void Execute(TCommand command);
	}
}