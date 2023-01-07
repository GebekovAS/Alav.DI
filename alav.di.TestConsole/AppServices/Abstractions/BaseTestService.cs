using Alav.DI.Attributes;

namespace Alav.DI.TestConsole.AppServices.Abstractions
{
    [ADI(Enums.ADIServiceLifetime.Singleton)]
    public abstract class BaseTestService
    {
        public abstract void Test();
    }
}
