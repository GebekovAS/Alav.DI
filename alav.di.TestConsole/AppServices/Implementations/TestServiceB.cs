using Alav.DI.TestConsole.AppServices.Abstractions;
using System;

namespace Alav.DI.TestConsole.AppServices.Implementations
{
    public class TestServiceB: BaseTestService
    {
        public override void Test()
        {
            Console.WriteLine($"{nameof(TestServiceB)}:Test");
        }
    }
}
