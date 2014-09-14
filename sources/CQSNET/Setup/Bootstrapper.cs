using System;
using System.Reflection;
using CQSNET.Mapping;

namespace CQSNET.Setup
{
	/// <summary>
	/// Facade class to register all components, including queries, commands, workflows and mappings.
	/// </summary>
	public class Bootstrapper
	{
		/// <summary>
		/// Register components containing in assmeblies.
		/// </summary>
		/// <param name="assemblies">List of assembiles, which contains compnents</param>
		/// <param name="registerContainerElement">Callback to register element in container</param>
		/// <param name="mapper">Container for mappings (optional)</param>
		public static void Setup(string[] assemblies, Action<Type, Type> registerContainerElement, IMapper mapper = null)
		{
			Setup(assemblies.GetAssemblies(), registerContainerElement, mapper);
		}

		/// <summary>
		/// Register components containing in assmeblies.
		/// </summary>
		/// <param name="assemblies">List of assembiles, which contains compnents</param>
		/// <param name="registerContainerElement">Callback to register element in container</param>
		/// <param name="mapper">Container for mappings (optional)</param>
		public static void Setup(Assembly[] assemblies, Action<Type, Type> registerContainerElement, IMapper mapper = null)
		{
			QueryBootstrapper.Register(assemblies, registerContainerElement);
			WorkflowBootstrapper.Register(assemblies, registerContainerElement);
			CommandsBootstrapper.Register(assemblies, registerContainerElement);

			if (mapper != null)
				MappingsBootstrapper.Register(assemblies, mapper);
		}
	}
}