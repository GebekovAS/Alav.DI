using Alav.DI.TestConsole.AppServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alav.DI.TestConsole.AppServices.Implementations
{
    public class TestServiceC : ITestService
    {
        public void Test()
        {
            Console.WriteLine($"{nameof(TestServiceC)}:Test");
        }
    }
}
