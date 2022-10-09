﻿using Alav.DI.TestConsole.AppServices.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alav.DI.TestConsole.AppServices.Implementations
{
    public class TestServiceA : BaseTestService
    {
        public override void Test()
        {
            Console.WriteLine($"{nameof(TestServiceA)}:Test");
        }
    }
}
