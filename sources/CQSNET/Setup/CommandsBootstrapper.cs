using System;
using System.Reflection;

namespace CQSNET.Setup
{
	/// <summary>
	/// Bootstrapper for commands
	/// </summary>
	public class CommandsBootstrapper
	{
		private readonly Action<Type, Type> _registerContainerElement;

		public CommandsBootstrapper(Action<Type, Type> registerContainerElement)
		{
			if (registerContainerElement == null)
				throw new ArgumentNullException("registerContainerElement");

			_registerContainerElement = registerContainerElement;
		}

		/// <summary>
		/// Register all command handlers declared in assemblies
		/// </summary>
		/// <param name="assemblies">List of assemblies, which contains components</param>
		public void Register(string[] assemblies)
		{
			Register(assemblies.GetAssemblies());
		}

		/// <summary>
		/// Register all command handlers declared in assemblies
		/// </summary>
		/// <param name="assemblies">List of assemblies, which contains components</param>
		public void Register(Assembly[] assemblies)
		{
			foreach (var command in assemblies.GetIntefaceImplementationsWithWrapper<ICommand>(typeof(ICommandHandler<>)))
			{
				foreach (Type commandHandler in command.Value)
				{
					_registerContainerElement(command.Key, commandHandler);
				}
			}
		}

		/// <summary>
		/// Register all command handlers declared in assemblies
		/// </summary>
		/// <param name="assemblies">List of assemblies, which contains components</param>
		/// <param name="registerContainerElement">Callback to register element in container</param>
		public static void Register(Assembly[] assemblies, Action<Type, Type> registerContainerElement)
		{
			CommandsBootstrapper bootstrapper = new CommandsBootstrapper(registerContainerElement);
			bootstrapper.Register(assemblies);
		}

		/// <summary>
		/// Register all command handlers declared in assemblies
		/// </summary>
		/// <param name="assemblies">List of assemblies, which contains components</param>
		/// <param name="registerContainerElement">Callback to register element in container</param>
		public static void Register(string[] assemblies, Action<Type, Type> registerContainerElement)
		{
			CommandsBootstrapper bootstrapper = new CommandsBootstrapper(registerContainerElement);
			bootstrapper.Register(assemblies);
		}
	}
}