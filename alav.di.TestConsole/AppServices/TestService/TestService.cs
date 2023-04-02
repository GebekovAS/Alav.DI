using Alav.DI.Attributes;
using Alav.DI.Enums;
using ConsoleTest.AppServices.PingService;
using Microsoft.Extensions.Logging;

namespace ConsoleTest.AppServices.TestService
{
    [ADI(ADIServiceLifetime.Scoped,  typeof(ITestService), typeof(TestService))]
    public class TestService : ITestService
    {
#pragma warning disable 0649

        [ADIInject]
        private readonly ILogger<TestService> _logger;

#pragma warning restore 0649

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
