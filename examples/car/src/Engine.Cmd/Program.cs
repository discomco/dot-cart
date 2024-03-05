using Engine.Context;
using Serilog;
using Inject = DotCart.Logging.Inject;

//DotEnv.FromEmbedded();


var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
        logging
            .ClearProviders()
            .AddSerilog())
    .UseSerilog(Inject.CreateSerilogConsoleLogger())
    .ConfigureServices(services =>
    {
        services
            .AddCartwheel();
    })
    .Build();


host.Run();