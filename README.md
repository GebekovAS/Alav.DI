# Alav.DI [![Build status](https://ci.appveyor.com/api/projects/status/vamv8y7w2lyu7wc3?svg=true)](https://ci.appveyor.com/project/GebekovAS/alav-di) [![NuGet Package](https://img.shields.io/nuget/v/Alav.DI.svg?v=1.0.4)](https://www.nuget.org/packages/Alav.DI)

> Alav.DI - additional Tools for Working with Dependency Injection. The tool allows you to inject service dependencies using annotations. This will allow you to set the lifetime, for example, directly in the service class itself. If you set the lifetime for an interface or an abstract class, then it will be applied automatically to all descendants.

## Installation

Install the [NuGet Package](https://www.nuget.org/packages/Alav.DI).

### Package Manager Console

```
Install-Package Alav.DI
```

### .NET Core CLI

```
dotnet add package Alav.DI
```

## Usage

The library adds extension method to `IServiceCollection`:

* `Scan` - This is the entry point to set up your assembly scanning.

See **Examples** below for usage examples.

## Examples

### Scanning

```csharp
    public interface IPingService
    {
        void Ping();
    }
    ...

    using Alav.DI.Attributes;
    //Annotation for class inclusion in scan results
    [ADI(Alav.DI.Enums.ADIServiceLifetime.Singleton, typeof(IPingService), typeof(PingService))]
    public class PingService : IPingService
    {
        public void Ping()
        {
            Console.WriteLine($"{nameof(PingService)}:Ping");
        }
    }
    ...

    using Alav.DI.Extensions;
    ...
    var services = new ServiceCollection()
                            .Scan<Program>()
                            .BuildServiceProvider();
    var pingService = services.GetService<IPingService>();
    pingService.Ping();
```

### Dependency Injection
* `ADIInjectAttribute` - this attribute marks the fields that need to be injected dynamically (without explicit declaration in the constructor)

```csharp
    [ADI(ADIServiceLifetime.Scoped, typeof(ITestService))]
    public class TestService : ITestService
    {
        [ADIInject]
        private readonly ILogger<TestService> _logger;

        [ADIInject]
        private readonly IPingService _pingService;

        public void Ping()
        {
            _logger.LogInformation($"{nameof(TestService)}:Ping");
            _pingService.Ping();
        }
    }
```
