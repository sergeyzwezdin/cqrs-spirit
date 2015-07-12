using CQSNET.Mapping;
using System;
using System.Linq;
using System.Reflection;

namespace CQSNET.Setup
{
    /// <summary>
    /// Bootstrapper for mappings
    /// </summary>
    public class MappingsBootstrapper
    {
        private readonly IMapper _mapper;

        public MappingsBootstrapper(IMapper mapper)
        {
            if (mapper == null)
                throw new ArgumentNullException("mapper");

            _mapper = mapper;
        }

        /// <summary>
        /// Register all mappings declared in assemblies
        /// </summary>
        /// <param name="assemblies">List of assemblies, which contains components</param>
        public void Register(string[] assemblies)
        {
            Register(assemblies.GetAssemblies());
        }

        /// <summary>
        /// Register all mappings declared in assemblies
        /// </summary>
        /// <param name="assemblies">List of assemblies, which contains components</param>
        public void Register(Assembly[] assemblies)
        {
            foreach (var mapping in assemblies
                .GetGenericIntefaceImplementations(typeof(IMapping<,>))
                .Select(Activator.CreateInstance))
            {
                _mapper.RegisterMapper(mapping);
            }
        }

        /// <summary>
        /// Register all mappings declared in assemblies
        /// </summary>
        /// <param name="assemblies">List of assemblies, which contains components</param>
        /// <param name="mapper">Container for mappings</param>
        public static void Register(Assembly[] assemblies, IMapper mapper)
        {
            MappingsBootstrapper bootstrapper = new MappingsBootstrapper(mapper);
            bootstrapper.Register(assemblies);
        }

        /// <summary>
        /// Register all mappings declared in assemblies
        /// </summary>
        /// <param name="assemblies">List of assemblies, which contains components</param>
        /// <param name="mapper">Container for mappings</param>
        public static void Register(string[] assemblies, IMapper mapper)
        {
            MappingsBootstrapper bootstrapper = new MappingsBootstrapper(mapper);
            bootstrapper.Register(assemblies);
        }
    }
}