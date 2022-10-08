using Alav.DI.Attributes;
using Alav.DI.Enums;
using ConsoleTest.AppServices.PingService;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTest.AppServices.TestService
{
    [ADI(ServiceLifetime = ADIServiceLifetime.Scoped, Interface = typeof(ITestService))]
    public class TestService : ITestService
    {
        private readonly ILogger<TestService> _logger;
        private readonly IPingService _pingService;
        public TestService(ILogger<TestService> logger, IPingService pingService)
        {
            _logger = logger;
            _pingService = pingService;
        }

        public void Ping()
        {
            _logger.LogInformation($"{nameof(TestService)}:Ping");
            _pingService.Ping();
        }
    }
}
