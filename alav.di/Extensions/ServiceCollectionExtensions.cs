using Alav.DI.Attributes;
using Alav.DI.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alav.DI.Extensions
{
    /// <summary>
    /// Extensions - IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Scan all AppDomain Assemblies class with attribute ADIServiceLifetime
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="expression">Search expression</param>
        /// <returns>ServiceCollection</returns>
        public static IServiceCollection Scan(this IServiceCollection services, Func<Type, bool> expression = null)
        {
            AppDomain.CurrentDomain.AssemblyLoad += (sender, args) =>
            {
                services.ScanAssembly(args.LoadedAssembly);
            };

            foreach (var assemply in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    services = services.ScanAssembly(assemply, expression);
                }
                catch (ReflectionTypeLoadException) { }
            }

            return services;
        }


        /// <summary>
        /// Scan all class with attribute ADIServiceLifetime by class assebmly
        /// </summary>
        /// <typeparam name="T">Assembly class to look for</typeparam>
        /// <param name="services">Service collection</param>
        /// <param name="expression">Search expression</param>
        /// <returns>ServiceCollection</returns>
        public static IServiceCollection Scan<T>(this IServiceCollection services, Func<Type, bool> expression = null) => services.ScanAssembly(typeof(T).Assembly, expression);

        private static IServiceCollection ScanAssembly(this IServiceCollection services, Assembly assembly, Func<Type, bool> expression = null)
        {
            Dictionary<Type, IEnumerable<ADIAttribute>> processedTypes = new();
            void tryAddTypeAttributesAction(Type type, IEnumerable<ADIAttribute> attributes)
            {
                if (!processedTypes.ContainsKey(type))
                {
                    processedTypes.Add(type, attributes);
                }
            }

            assembly
                .GetTypes()
                .Where(item => item.GetCustomAttributes<ADIAttribute>().Any() && (expression == null || expression(item)))
                .ToList()
                .ForEach(serviceType =>
                {
                    var attrs = serviceType.GetCustomAttributes<ADIAttribute>();

                    if (processedTypes.ContainsKey(serviceType))
                    {
                        return;
                    }

                    if (serviceType.IsAbstract || serviceType.IsInterface)
                    {
                        var inheritedTypes = GetInheritedClasses(serviceType);
                        foreach (var inheritedType in inheritedTypes)
                        {
                            tryAddTypeAttributesAction(inheritedType, attrs);
                        }
                    }
                    else
                    {
                        tryAddTypeAttributesAction(serviceType, attrs);
                    }
                });



            foreach (var type in processedTypes)
            {
                foreach (var attr in type.Value)
                {
                    services.AddService(type.Key, attr);
                }
            }

            return services;
        }

        private static void AddService(this IServiceCollection services, Type serviceType, ADIAttribute attributes)
        {
            switch (attributes.ServiceLifetime)
            {
                case Enums.ADIServiceLifetime.Singleton:
                    if (attributes.ServiceTypes?.Any() ?? false)
                    {
                        foreach (var customServiceType in attributes.ServiceTypes)
                        {
                            services.TryAddSingleton(customServiceType, sp => CreateInstance(sp, serviceType));
                        }
                    }
                    else
                    {
                        services.TryAddSingleton(serviceType, sp => CreateInstance(sp, serviceType));
                    }
                    break;
                case Enums.ADIServiceLifetime.Transient:
                    if (attributes.ServiceTypes?.Any() ?? false)
                    {
                        foreach (var customServiceType in attributes.ServiceTypes)
                        {
                            services.TryAddTransient(customServiceType, sp => CreateInstance(sp, serviceType));
                        }
                    }
                    else
                    {
                        services.TryAddTransient(serviceType, sp => CreateInstance(sp, serviceType));
                    }
                    break;
                case Enums.ADIServiceLifetime.Scoped:
                    if (attributes.ServiceTypes?.Any() ?? false)
                    {
                        foreach (var customServiceType in attributes.ServiceTypes)
                        {
                            services.TryAddScoped(customServiceType, sp => CreateInstance(sp, serviceType));
                        }
                    }
                    else
                    {
                        services.TryAddScoped(serviceType, sp => CreateInstance(sp, serviceType));
                    }
                    break;
            }
        }

        #region Add

        private static object CreateInstance(IServiceProvider provider, Type serviceType)
        {
            var constructor = serviceType
                                .GetConstructors()
                                .FirstOrDefault();

            var constructorParams = constructor.GetParameters();
            var args = new object[constructorParams.Length];
            for (var index = 0; index < constructorParams.Length; index++)
            {
                args[index] = provider.GetRequiredService(constructorParams[index].ParameterType);
            }

            var instance = Assembly
                .GetAssembly(serviceType)
                .CreateInstance(serviceType.FullName, true, BindingFlags.Instance | BindingFlags.Public, null, args, null, null);

            var fields = serviceType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                            .Where(field => field.GetCustomAttribute<ADIInjectAttribute>() != null);
            foreach (var field in fields)
            {
                var fieldInstance = provider.GetRequiredService(field.FieldType);
                field.SetValue(instance, fieldInstance);
            }


            return instance;
        }

        #endregion

        private static IEnumerable<Type> GetInheritedClasses(Type baseType)
        {
            return baseType.Assembly.GetTypes().Where(type => type.IsClass && !type.IsAbstract && (type.IsSubclassOf(baseType) || baseType.IsAssignableFrom(type)));
        }
    }
}
