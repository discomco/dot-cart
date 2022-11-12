using DotCart.Core;
using DotCart.Drivers.Serilog;
using Engine.TestConsole;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Inject = DotCart.Drivers.Serilog.Inject;

DotEnv.FromEmbedded();

var ts = new CancellationTokenSource();

var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
        services
            .AddConsoleLogger()
            .BuildTestApp()
    );
var host = hostBuilder.ConfigureLogging(logging => logging
        .ClearProviders()
        .AddSerilog())
    .UseSerilog(Inject.CreateSeriLogConsoleLogger())
    .Build();

await host
    .RunAsync(ts.Token);

//host.Run();