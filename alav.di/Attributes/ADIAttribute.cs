using Alav.DI.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alav.DI.Attributes
{
    /// <summary>
    /// Attribute for class inclusion in scan results
    /// </summary>
    public class ADIAttribute: Attribute
    {
        /// <summary>
        /// Service lifetime (Singleton, Transient ...)
        /// </summary>
        public ADIServiceLifetime ServiceLifetime { get; set; }

        /// <summary>
        /// Service interface (DI)
        /// </summary>
        public Type? Interface { get; set; }
    }
}
