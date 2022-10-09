using Alav.DI.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alav.DI.TestConsole.AppServices.Abstractions
{
    [ADI(ServiceLifetime = Enums.ADIServiceLifetime.Singleton)]
    public abstract class BaseTestService
    {
        public abstract void Test();
    }
}
