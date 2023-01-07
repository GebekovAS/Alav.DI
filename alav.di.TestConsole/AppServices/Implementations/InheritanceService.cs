using Alav.DI.Attributes;
using Alav.DI.TestConsole.AppServices.Abstractions;
using ConsoleTest.AppServices.PingService;
using System;

namespace Alav.DI.TestConsole.AppServices.Implementations
{
    /// <summary>
    /// Service - An example of inheritance by inheriting from a base class with an ADI attribute
    /// </summary>
    public class InheritanceService : BaseTestService
    {
        [ADIInject]
        private IPingService _pingService;

        public override void Test()
        {
            Console.WriteLine($"{nameof(InheritanceService)}:Test");
            _pingService.Ping();
        }
    }
}
