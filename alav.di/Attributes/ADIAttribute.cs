using Alav.DI.Enums;
using System;

namespace Alav.DI.Attributes
{
    /// <summary>
    /// Attribute for class inclusion in scan results
    /// </summary>
    public class ADIAttribute : Attribute
    {
        public ADIAttribute(ADIServiceLifetime serviceLifetime, params Type[] serviceType)
        {
            ServiceLifetime = serviceLifetime;
            ServiceTypes = serviceType;
        }

        /// <summary>
        /// Service lifetime (Singleton, Transient ...)
        /// </summary>
        public ADIServiceLifetime ServiceLifetime { get; set; }

        /// <summary>
        /// Service type (DI)
        /// </summary>
        public Type[] ServiceTypes { get; set; }
    }
}
