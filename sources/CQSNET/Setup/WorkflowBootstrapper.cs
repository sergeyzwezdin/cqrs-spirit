using System;
using System.Reflection;

namespace CQSNET.Setup
{
	/// <summary>
	/// Bootstrapper for workflows
	/// </summary>
	public class WorkflowBootstrapper
	{
		private readonly Action<Type, Type> _registerContainerElement;

		public WorkflowBootstrapper(Action<Type, Type> registerContainerElement)
		{
			if (registerContainerElement == null)
				throw new ArgumentNullException("registerContainerElement");

			_registerContainerElement = registerContainerElement;
		}

		/// <summary>
		/// Register all workflows declared in assemblies
		/// </summary>
		/// <param name="assemblies">List of assemblies, which contains components</param>
		public void Register(string[] assemblies)
		{
			Register(assemblies.GetAssemblies());
		}

		/// <summary>
		/// Register all workflows declared in assemblies
		/// </summary>
		/// <param name="assemblies">List of assemblies, which contains components</param>
		public void Register(Assembly[] assemblies)
		{
			foreach (var workflow in assemblies.GetIntefaceImplementations<IWorkflow>())
			{
				_registerContainerElement(workflow.Key, workflow.Value);
			}
		}

		/// <summary>
		/// Register all workflows declared in assemblies
		/// </summary>
		/// <param name="assemblies">List of assemblies, which contains components</param>
		/// <param name="registerContainerElement">Callback to register element in container</param>
		public static void Register(Assembly[] assemblies, Action<Type, Type> registerContainerElement)
		{
			WorkflowBootstrapper bootstrapper = new WorkflowBootstrapper(registerContainerElement);
			bootstrapper.Register(assemblies);
		}

		/// <summary>
		/// Register all workflows declared in assemblies
		/// </summary>
		/// <param name="assemblies">List of assemblies, which contains components</param>
		/// <param name="registerContainerElement">Callback to register element in container</param>
		public static void Register(string[] assemblies, Action<Type, Type> registerContainerElement)
		{
			WorkflowBootstrapper bootstrapper = new WorkflowBootstrapper(registerContainerElement);
			bootstrapper.Register(assemblies);
		}
	}
}