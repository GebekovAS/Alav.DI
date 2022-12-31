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
        public static IServiceCollection Scan(this IServiceCollection services, Func<Type, bool>? expression = null)
        {
            foreach (var assemply in AppDomain.CurrentDomain.GetAssemblies())
            {
                services = services.ScanAssembly(assemply, expression);
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
        public static IServiceCollection Scan<T>(this IServiceCollection services, Func<Type, bool>? expression = null) => services.ScanAssembly(typeof(T).Assembly, expression);

        private static IServiceCollection ScanAssembly(this IServiceCollection services, Assembly assembly, Func<Type, bool>? expression = null)
        {
            Dictionary<Type, IEnumerable<ADIAttribute>> processedTypes = new Dictionary<Type, IEnumerable<ADIAttribute>>();
            Action<Type, IEnumerable<ADIAttribute>> tryAddTypeAttributesAction = (type, attributes) =>
            {
                if (!processedTypes.ContainsKey(type))
                {
                    processedTypes.Add(type, attributes);
                }
            };

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
                    services.AddSingleton(attributes.Interface ?? serviceType, sp => CreateInstance(sp, serviceType));
                    break;
                case Enums.ADIServiceLifetime.Transient:
                    services.AddTransient(attributes.Interface ?? serviceType, sp => CreateInstance(sp, serviceType));
                    break;
                case Enums.ADIServiceLifetime.Scoped:
                    services.AddScoped(attributes.Interface ?? serviceType, sp => CreateInstance(sp, serviceType));
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
                args[index] = provider.GetService(constructorParams[index].ParameterType);
            }

            var instance = Assembly
                .GetAssembly(serviceType)
                .CreateInstance(serviceType.FullName, true, BindingFlags.Instance | BindingFlags.Public, null, args, null, null);

            var fields = serviceType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                            .Where(field => field.GetCustomAttribute<ADIInjectAttribute>() != null);
            foreach (var field in fields)
            {
                var fieldInstance = provider.GetService(field.FieldType);
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
