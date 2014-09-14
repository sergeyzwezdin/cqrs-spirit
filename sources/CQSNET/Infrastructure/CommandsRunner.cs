using System;
using System.Collections.Generic;
using System.Linq;

namespace CQSNET.Infrastructure
{
	public class CommandsRunner : ICommandsRunner
	{
		private readonly Func<Type, IEnumerable<object>> _resolveCallback;


		public CommandsRunner(Func<Type, IEnumerable<object>> resolveCallback)
		{
			_resolveCallback = resolveCallback;
		}


		public void Execute<T>(T command)
			where T : class, ICommand
		{
			// Initialize context
			IEnumerable<ICommandHandler<T>> commandHandlers =
				_resolveCallback(typeof(ICommandHandler<T>))
				.OfType<ICommandHandler<T>>()
				.ToArray();


			if (commandHandlers.Any())
			{
				foreach (ICommandHandler<T> commandHandler in commandHandlers)
				{
					// Execute command
					commandHandler.Execute(command);


					// Dispose context
					commandHandler.Dispose();
				}
			}
			else
				throw new ArgumentException("Unknown command \"" + typeof(T).FullName + "\"");
		}
	}
}