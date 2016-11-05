using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System.Collections.Generic;

namespace CqrsSpirit
{
    public static class CqrsSpiritExtensions
    {
        private static readonly Assembly CurrentAssembly = typeof(IQuery).GetTypeInfo().Assembly;

        /// <summary>
        /// Registers CQRS chunks into service container
        /// </summary>
        /// <param name="services">DI container</param>
        /// <param name="filter">Predicate to filter types you would like to register</param>
        public static IServiceCollection AddCqrsSpirit(this IServiceCollection services, Func<Type, bool> filter = null)
        {
            return AddCqrsSpirit(services, DependencyContext.Default, filter);
        }

        /// <summary>
        /// Registers CQRS chunks into service container
        /// </summary>
        /// <param name="services">DI container</param>
        /// <param name="dependencyContext">Dependencies</param>
        /// <param name="filter">Predicate to filter types you would like to register</param>
        public static IServiceCollection AddCqrsSpirit(this IServiceCollection services, DependencyContext dependencyContext, Func<Type, bool> filter = null)
        {
            var assemblies = dependencyContext.RuntimeLibraries
                .Where(library => library.Dependencies.Any(dependency => dependency.Name.StartsWith(CurrentAssembly.GetName().Name, StringComparison.OrdinalIgnoreCase)))
                .SelectMany(library => library.GetDefaultAssemblyNames(dependencyContext))
                .Select(Assembly.Load)
                .ToArray();

            return AddCqrsSpirit(services, assemblies, filter);
        }

        /// <summary>
        /// Registers CQRS chunks into service container
        /// </summary>
        /// <param name="services">DI container</param>
        /// <param name="assemblies">Assemblies list where is CQRS stuff is placed</param>
        /// <param name="filter">Predicate to filter types you would like to register</param>
        public static IServiceCollection AddCqrsSpirit(this IServiceCollection services, Assembly[] assemblies, Func<Type, bool> filter = null)
        {
            var types = assemblies
                .SelectMany(assembly => assembly.ExportedTypes)
                .Where(t => t.GetTypeInfo().IsPublic);

            if (filter != null)
                types = types.Where(filter);

            return AddCqrsSpirit(services, types.ToArray());
        }

        /// <summary>
        /// Registers CQRS chunks into service container
        /// </summary>
        /// <param name="services">DI container</param>
        /// <param name="types">List of types to register</param>
        public static IServiceCollection AddCqrsSpirit(this IServiceCollection services, Type[] types)
        {
            var queryDefinition = typeof(IQuery).GetTypeInfo();
            var commandDefinition = typeof(ICommand).GetTypeInfo();
            var workflowDefinition = typeof(IWorkflow).GetTypeInfo();

            var queries = MatchTypes(types, queryDefinition);
            var commands = MatchTypesByGenerics(types, typeof(ICommandHandler<>).GetTypeInfo(), commandDefinition);
            var workflows = MatchTypes(types, workflowDefinition);

            return AddCqrsSpirit(services, queries, commands, workflows);
        }

        /// <summary>
        /// Registers CQRS chunks into service container
        /// </summary>
        /// <param name="services">DI container</param>
        /// <param name="queries">List of queries to register</param>
        /// <param name="commands">List of commands to register</param>
        /// <param name="workflows">List of workflows to register</param>
        public static IServiceCollection AddCqrsSpirit(this IServiceCollection services, IDictionary<Type, Type> queries, IDictionary<Type, Type> commands, IDictionary<Type, Type> workflows)
        {
            services.AddSingleton<ICommandsDispatcher, CommandsDispatcher>();

            if (queries != null)
            {
                foreach (var query in queries)
                {
                    if (query.Value != null)
                        services.AddTransient(query.Key, query.Value);
                }
            }

            if (commands != null)
            {
                foreach (var command in commands)
                {
                    if (command.Value != null)
                        services.AddTransient(command.Key, command.Value);
                }
            }

            if (workflows != null)
            {
                foreach (var workflow in workflows)
                {
                    if (workflow.Value != null)
                        services.AddTransient(workflow.Key, workflow.Value);
                }
            }

            return services;
        }

        private static IDictionary<Type, Type> MatchTypes(Type[] types, TypeInfo baseTypeDefinition)
        {
            return types
                    .Where(t => t.GetTypeInfo().IsInterface && baseTypeDefinition.IsAssignableFrom(t.GetTypeInfo())).ToArray()
                    .ToDictionary(x => x,
                                  x =>
                                  {
                                      var result = types.Where(t => (t != x) && x.GetTypeInfo().IsAssignableFrom(t.GetTypeInfo())).ToArray();

                                      if (result.Length > 1)
                                          throw new AmbiguousMatchException($"Ambiguous match found for {x.FullName}: {String.Join(", ", result.Select(t => t.FullName))}");

                                      return result.FirstOrDefault();
                                  });
        }

        private static IDictionary<Type, Type> MatchTypesByGenerics(Type[] types, TypeInfo genericTypeDefinition, TypeInfo baseTypeDefinition)
        {
            return types
                    .Where(t => t.GetTypeInfo().IsClass && baseTypeDefinition.IsAssignableFrom(t.GetTypeInfo())).ToArray()
                    .ToDictionary(x => genericTypeDefinition.MakeGenericType(x),
                                  x =>
                                  {
                                      var genericTypeBase = genericTypeDefinition.MakeGenericType(x);
                                      var result = types.Where(t => genericTypeBase.GetTypeInfo().IsAssignableFrom(t.GetTypeInfo())).ToArray();

                                      if (result.Length > 1)
                                          throw new AmbiguousMatchException($"Ambiguous match found for {x.FullName}: {String.Join(", ", result.Select(t => t.FullName))}");

                                      return result.FirstOrDefault();
                                  });
        }
    }
}
