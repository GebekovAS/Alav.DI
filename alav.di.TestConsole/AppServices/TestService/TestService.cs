using Alav.DI.Attributes;
using Alav.DI.Enums;
using ConsoleTest.AppServices.PingService;
using Microsoft.Extensions.Logging;

namespace ConsoleTest.AppServices.TestService
{
    [ADI(ServiceLifetime = ADIServiceLifetime.Scoped, Interface = typeof(ITestService))]
    public class TestService : ITestService
    {
        [ADIInject]
        private readonly ILogger<TestService> _logger;

        private readonly IPingService _pingService;

        public TestService(IPingService pingService)
        {
            _pingService = pingService;
        }

        public void Ping()
        {
            _logger.LogInformation($"{nameof(TestService)}:Ping");
            _pingService.Ping();
        }
    }
}
