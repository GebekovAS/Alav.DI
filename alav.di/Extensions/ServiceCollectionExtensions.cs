using Alav.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;
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

        private static IServiceCollection ScanAssembly(this IServiceCollection services, Assembly assembly, Func<Type,bool>? expression = null)
        {
            assembly
                .GetTypes()
                .Where(item => item.GetCustomAttributes<ADIAttribute>().Any() && (expression == null || expression(item)))
                .ToList()
                .ForEach(assemblyType =>
                {
                    var attrs = assemblyType.GetCustomAttributes<ADIAttribute>();
                    foreach (var attr in attrs)
                    {
                        switch (attr.ServiceLifetime)
                        {
                            case Enums.ADIServiceLifetime.Singleton:
                                if (attr.Interface != null)
                                {
                                    services.AddSingleton(attr.Interface, assemblyType);
                                }
                                else
                                {
                                    services.AddSingleton(assemblyType);
                                }
                                break;
                            case Enums.ADIServiceLifetime.Transient:
                                if (attr.Interface != null)
                                {
                                    services.AddTransient(attr.Interface, assemblyType);
                                }
                                else
                                {
                                    services.AddTransient(assemblyType);
                                }
                                break;
                            case Enums.ADIServiceLifetime.Scoped:
                                if (attr.Interface != null)
                                {
                                    services.AddScoped(attr.Interface, assemblyType);
                                }
                                else
                                {
                                    services.AddScoped(assemblyType);
                                }
                                break;
                        }
                    }
                });

            return services;
        }
    }
}
