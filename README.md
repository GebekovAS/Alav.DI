# Alav.DI [![Build status](https://ci.appveyor.com/api/projects/status/vamv8y7w2lyu7wc3?svg=true)](https://ci.appveyor.com/project/GebekovAS/alav-di) [![NuGet Package](https://img.shields.io/nuget/v/Alav.DI.svg)](https://www.nuget.org/packages/Alav.DI)

> Alav.DI - additional Tools for Working with Dependency Injection

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
    [ADI(ServiceLifetime = Alav.DI.Enums.ADIServiceLifetime.Singleton, Interface = typeof(IPingService))]
    public class PingService : IPingService
    {
        private readonly ILogger<PingService> _logger;
        public PingService(ILogger<PingService> logger) => _logger = logger;

        public void Ping()
        {
            _logger.LogInformation($"{nameof(PingService)}:Ping");
        }
    }
    ...

    using Alav.DI.Extensions;
    ...
    var services = new ServiceCollection()
                            .AddLogging(opt =>
                            {
                                opt.AddConsole();
                                opt.AddJsonConsole();
                            })
                            .Scan<Program>()
                            .BuildServiceProvider();
    var pingService = services.GetService<IPingService>();
    pingService.Ping();
```
