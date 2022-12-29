using Alav.DI.Attributes;

namespace Alav.DI.TestConsole.AppServices.Interfaces
{
    [ADI(ServiceLifetime = Enums.ADIServiceLifetime.Singleton)]
    public interface ITestService
    {
        void Test();
    }
}
