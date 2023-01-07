using Alav.DI.Extensions;
using Alav.DI.TestConsole.AppServices.Implementations;
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

            var testService1 = services.GetService<ITestService>();
            testService1.Ping();
            var testService2 = services.GetService<TestService>();
            testService2.Ping();
            var testServiceA = services.GetService<InheritanceService>();
            testServiceA.Test();
            var testServiceB = services.GetService<TestServiceB>();
            testServiceB.Test();
            var testServiceC = services.GetService<TestServiceC>();
            testServiceC.Test();

            Console.ReadKey();

        }
    }
}
