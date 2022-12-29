using Alav.DI.Extensions;
using Alav.DI.TestConsole.AppServices.Implementations;
using Alav.DI.TestConsole.AppServices.TestDI;
using ConsoleTest.AppServices.PingService;
using ConsoleTest.AppServices.TestService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace ConsoleTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection()
                            .AddLogging(opt =>
                            {
                                opt.AddConsole();
                                opt.AddJsonConsole();
                            })
                            .Scan<Program>()
                            .BuildServiceProvider();
            var testService = services.GetService<ITestService>();
            testService.Ping();

            var testServiceA = services.GetService<TestServiceA>();
            testServiceA.Test();
            var testServiceB = services.GetService<TestServiceB>();
            testServiceB.Test();
            var testServiceC = services.GetService<TestServiceC>();
            testServiceC.Test();
            var testServiceD = services.GetService<ITestServiceD>();
            testServiceD.Test();

            Console.ReadKey();

        }
    }
}
