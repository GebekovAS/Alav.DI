using Alav.DI.Extensions;
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

            Console.ReadKey();

        }
    }
}
