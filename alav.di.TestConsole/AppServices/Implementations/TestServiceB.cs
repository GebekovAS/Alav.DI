using Alav.DI.TestConsole.AppServices.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

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
