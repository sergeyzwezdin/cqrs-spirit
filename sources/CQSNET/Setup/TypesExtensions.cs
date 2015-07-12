using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CQSNET.Setup
{
    /// <summary>
    /// Helper class to work with types
    /// </summary>
    public static class TypesExtensions
    {
        /// <summary>
        /// Returns asseblies by names
        /// </summary>
        /// <param name="assembliesNames">Names of assemblies</param>
        /// <returns>Assemblies definition list</returns>
        public static Assembly[] GetAssemblies(this IEnumerable<string> assembliesNames)
        {
            Assembly[] assemblies = assembliesNames
                .Select(Assembly.Load)
                .ToArray();

            return assemblies;
        }

        /// <summary>
        /// Return object implementation by interface definition
        /// </summary>
        /// <typeparam name="TInterface">Interface type</typeparam>
        /// <param name="assemblies">Assemblies list to search</param>
        /// <returns>Dictionary of interface-implementation items</returns>
        public static IDictionary<Type, Type> GetIntefaceImplementations<TInterface>(this IEnumerable<Assembly> assemblies)
            where TInterface : class
        {
            if (assemblies == null)
                throw new ArgumentNullException("assemblies");

            Type interfaceType = typeof(TInterface);

            IEnumerable<Type> allTypes = assemblies
                .SelectMany(x => x.GetTypes())
                .ToArray();

            Dictionary<Type, Type> result = allTypes
                .Where(x => interfaceType.IsAssignableFrom(x) && (x.IsAbstract == false) && (x.IsInterface == false))
                .Where(x => x.GetInterfaces()
                    .Where(t => t != interfaceType)
                    .Any(interfaceType.IsAssignableFrom))
                .ToDictionary(x => x.GetInterfaces()
                    .Where(t => t != interfaceType)
                    .FirstOrDefault(interfaceType.IsAssignableFrom),
                    x => x)
                .Where(x => (x.Key != null) && (x.Value != null))
                .ToDictionary(x => x.Key, x => x.Value);

            return result;
        }

        /// <summary>
        /// Return object implementation by interface definition
        /// </summary>
        /// <param name="assemblies">Assemblies list to search</param>
        /// <param name="interfaceType">Interface type</param>
        /// <returns>Types</returns>
        public static IEnumerable<Type> GetGenericIntefaceImplementations(this IEnumerable<Assembly> assemblies, Type interfaceType)
        {
            if (assemblies == null)
                throw new ArgumentNullException("assemblies");

            IEnumerable<Type> allTypes = assemblies
                .SelectMany(x => x.GetTypes())
                .ToArray();

            IEnumerable<Type> result = allTypes
                .Where(x => x.GetInterfaces().Any(i => (i.IsGenericType == true) && (interfaceType.IsGenericType == true) && (i.GetGenericTypeDefinition() == interfaceType.GetGenericTypeDefinition())))
                .ToArray();

            return result;
        }

        /// <summary>
        /// Return object implementation by interface definition
        /// </summary>
        /// <typeparam name="TInterface">Interface type</typeparam>
        /// <param name="assemblies">Assemblies list to search</param>
        /// <param name="wrappingType">Wrapping type</param>
        /// <returns>Dictionary of types</returns>
        public static IDictionary<Type, Type[]> GetIntefaceImplementationsWithWrapper<TInterface>(
            this IEnumerable<Assembly> assemblies, Type wrappingType)
            where TInterface : class
        {
            if (assemblies == null)
                throw new ArgumentNullException("assemblies");

            if (wrappingType == null)
                throw new ArgumentNullException("wrappingType");

            if (wrappingType.IsGenericType == false)
                throw new ArgumentException("Parameter wrappingType contains not generic type.");

            Type interfaceType = typeof(TInterface);

            IEnumerable<Type> allTypes = assemblies
                .SelectMany(x => x.GetTypes())
                .ToArray();

            Dictionary<Type, Type[]> result = allTypes
                .Where(x => interfaceType.IsAssignableFrom(x) && (x.IsAbstract == false) && (x.IsInterface == false))
                .Select(x => wrappingType.MakeGenericType(x))
                .ToDictionary(x => x,
                    x => allTypes
                        .Where(x.IsAssignableFrom)
                        .ToArray())
                .Where(x => (x.Value != null) && (x.Value.Any()))
                .ToDictionary(x => x.Key, x => x.Value);

            return result;
        }

        /// <summary>
        /// Extracts description from [Description] attribute of member
        /// </summary>
        /// <param name="reason">Initial object</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Content of [Description] attribute</returns>
        public static string ExtractDecription(this object reason, string defaultValue)
        {
            string result = defaultValue;

            MemberInfo member = reason
                .GetType()
                .GetMember(reason.ToString())
                .FirstOrDefault();

            if (member != null)
            {
                DescriptionAttribute attribute = Attribute.GetCustomAttribute(member, typeof(DescriptionAttribute)) as DescriptionAttribute;

                if (attribute != null)
                    result = attribute.Description;
            }

            return result;
        }

        /// <summary>
        /// Trying to parse enum value
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="source">Text representation of enum member</param>
        /// <returns>Enum member</returns>
        public static T? TryParseEnum<T>(this string source)
            where T : struct
        {
            T result;
            if (Enum.TryParse(source, true, out result))
                return result;
            else
                return null;
        }
    }
}