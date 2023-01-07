using Alav.DI.Attributes;

namespace Alav.DI.TestConsole.AppServices.Interfaces
{
    [ADI(Enums.ADIServiceLifetime.Singleton)]
    public interface ITestService
    {
        void Test();
    }
}
