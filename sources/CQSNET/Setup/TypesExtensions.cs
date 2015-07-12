using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CQSNET.Setup
{
    public static class TypesExtensions
    {
        public static Assembly[] GetAssemblies(this IEnumerable<string> assembliesNames)
        {
            Assembly[] assemblies = assembliesNames
                .Select(Assembly.Load)
                .ToArray();

            return assemblies;
        }

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