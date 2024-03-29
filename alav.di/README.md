﻿# Alav.DI

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
    
    //Annotation for class inclusion in scan results
    [ADI(Alav.DI.Enums.ADIServiceLifetime.Singleton, typeof(IPingService))]
    public class PingService : IPingService
    {
        [ADIInject]
        private readonly ILogger<PingService> _logger;

        public void Ping()
        {
            _logger.LogInformation($"{nameof(PingService)}:Ping");
        }
    }
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
