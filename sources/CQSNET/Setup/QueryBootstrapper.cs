using System;
using System.Reflection;

namespace CQSNET.Setup
{
	/// <summary>
	/// Bootstrapper for queries
	/// </summary>
	public class QueryBootstrapper
	{
		private readonly Action<Type, Type> _registerContainerElement;

		public QueryBootstrapper(Action<Type, Type> registerContainerElement)
		{
			if (registerContainerElement == null)
				throw new ArgumentNullException("registerContainerElement");

			_registerContainerElement = registerContainerElement;
		}

		/// <summary>
		/// Register all queries declared in assemblies
		/// </summary>
		/// <param name="assemblies">List of assemblies, which contains components</param>
		public void Register(string[] assemblies)
		{
			Register(assemblies.GetAssemblies());
		}

		/// <summary>
		/// Register all queries declared in assemblies
		/// </summary>
		/// <param name="assemblies">List of assemblies, which contains components</param>
		public void Register(Assembly[] assemblies)
		{
			foreach (var query in assemblies.GetIntefaceImplementations<IQuery>())
			{
				_registerContainerElement(query.Key, query.Value);
			}
		}

		/// <summary>
		/// Register all queries declared in assemblies
		/// </summary>
		/// <param name="assemblies">List of assemblies, which contains components</param>
		/// <param name="registerContainerElement">Callback to register element in container</param>
		public static void Register(Assembly[] assemblies, Action<Type, Type> registerContainerElement)
		{
			QueryBootstrapper bootstrapper = new QueryBootstrapper(registerContainerElement);
			bootstrapper.Register(assemblies);
		}

		/// <summary>
		/// Register all queries declared in assemblies
		/// </summary>
		/// <param name="assemblies">List of assemblies, which contains components</param>
		/// <param name="registerContainerElement">Callback to register element in container</param>
		public static void Register(string[] assemblies, Action<Type, Type> registerContainerElement)
		{
			QueryBootstrapper bootstrapper = new QueryBootstrapper(registerContainerElement);
			bootstrapper.Register(assemblies);
		}
	}
}