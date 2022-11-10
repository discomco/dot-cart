using DotCart.Drivers.Serilog;
using DotCart.TestEnv.CLI;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

var ts = new CancellationTokenSource();

var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
        services
            .AddConsoleLogger()
            .BuildTestApp()
    );
var host = hostBuilder.
    ConfigureLogging(logging => logging
        .ClearProviders()
        .AddSerilog())
    .UseSerilog(DotCart.Drivers.Serilog.Inject.CreateSeriLogConsoleLogger())
    .Build();

await host
    .RunAsync(ts.Token);