using Alav.DI.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alav.DI.TestConsole.AppServices.Interfaces
{
    [ADI(ServiceLifetime = Enums.ADIServiceLifetime.Singleton)]
    public interface ITestService
    {
        void Test();
    }
}
