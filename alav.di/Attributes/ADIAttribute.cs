using Alav.DI.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alav.DI.Attributes
{
    public class ADIAttribute: Attribute
    {
        public ADIServiceLifetime ServiceLifetime { get; set; }

        public Type? Interface { get; set; }
    }
}
