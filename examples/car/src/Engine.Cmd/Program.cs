using Engine.Context;
using Serilog;

//DotEnv.FromEmbedded();


var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
        logging
            .ClearProviders()
            .AddSerilog())
    .UseSerilog(DotCart.Logging.Inject.CreateSerilogConsoleLogger())
    .ConfigureServices(services =>
    {
        services
            .AddCartwheel();
    })
    .Build();


host.Run();