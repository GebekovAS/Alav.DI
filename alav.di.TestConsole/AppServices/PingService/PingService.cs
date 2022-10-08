using Alav.DI.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTest.AppServices.PingService
{
    [ADI(ServiceLifetime = Alav.DI.Enums.ADIServiceLifetime.Singleton, Interface = typeof(IPingService))]
    public class PingService : IPingService
    {
        private readonly ILogger<PingService> _logger;
        public PingService(ILogger<PingService> logger) => _logger = logger;

        public void Ping()
        {
            _logger.LogInformation($"{nameof(PingService)}:Ping");
        }
    }
}
