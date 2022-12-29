using Alav.DI.Attributes;

namespace Alav.DI.TestConsole.AppServices.Abstractions
{
    [ADI(ServiceLifetime = Enums.ADIServiceLifetime.Singleton)]
    public abstract class BaseTestService
    {
        public abstract void Test();
    }
}
