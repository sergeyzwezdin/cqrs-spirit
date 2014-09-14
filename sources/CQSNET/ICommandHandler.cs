using System;

namespace CQSNET
{
	public interface ICommandHandler<in TCommand> : IDisposable
		where TCommand : ICommand
	{
		void Execute(TCommand command);
	}
}