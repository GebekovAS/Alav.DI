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
#pragma warning disable 0649
        [ADIInject]
        private IPingService _pingService;
#pragma warning restore 0649

        public override void Test()
        {
            Console.WriteLine($"{nameof(InheritanceService)}:Test");
            _pingService.Ping();
        }
    }
}
