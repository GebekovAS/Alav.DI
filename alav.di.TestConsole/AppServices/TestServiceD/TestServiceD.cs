using Alav.DI.Attributes;
using ConsoleTest.AppServices.PingService;
using Microsoft.Extensions.Logging;

namespace Alav.DI.TestConsole.AppServices.TestDI
{
    [ADI<ITestServiceD>(ServiceLifetime = Enums.ADIServiceLifetime.Transient)]
    public class TestServiceD : ITestServiceD
    {
        private readonly ILogger<PingService> _logger;
        public TestServiceD(ILogger<PingService> logger) => _logger = logger;

        public void Test()
        {
            _logger.LogInformation($"{nameof(TestServiceD)}:Test");
        }
    }
}
