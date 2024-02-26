using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Core;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Microsoft.Extensions.Hosting;
using NATS.Client;
using Serilog;

namespace DotCart.Drivers.NATS;

public delegate Task ProcessFactAsync<in TPayload>(TPayload fact, CancellationToken cancellationToken)
    where TPayload : IPayload;

public class NATSListener<TPayload>
    : BackgroundService
    where TPayload : IPayload
{
    private readonly Action<Options> _configureOptions;
    private readonly INatsClientConnectionFactory _connectionFactory;
    private readonly ILogger _logger;
    private readonly ProcessFactAsync<TPayload> _processFunc;
    private IEncodedConnection? _bus;
    private string? _logMessage;
    private IAsyncSubscription? _subscription;

    public NATSListener(
        INatsClientConnectionFactory connectionFactory,
        Action<Options> configureOptions,
        ILogger logger,
        ProcessFactAsync<TPayload> processFunc)
    {
        _connectionFactory = connectionFactory;
        _configureOptions = configureOptions;
        _logger = logger;
        _processFunc = processFunc;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            await ConnectAsync(cancellationToken)
                .ConfigureAwait(false);
            _logger.Information($"::SUBSCRIBE ::Topic: [{FactTopicAtt.Get<TPayload>()}] on bus [{_bus.ConnectedId}]");
            _subscription = _bus.SubscribeAsync(
                FactTopicAtt.Get<TPayload>(),
                async (_, args) =>
                {
                    var fact = (TPayload)args.ReceivedObject;
                    try
                    {
                        _logger.Information($"::RECEIVED: {fact}");
                        await _processFunc(fact, cancellationToken)
                            .ConfigureAwait(false);
                    }
                    catch (Exception e)
                    {
                        _logger.Error($"::ERROR: {e.InnerAndOuter()}");
                    }

                    _logger.Debug($"::COMPLETED: {fact}");
                });
        }
        catch (Exception e)
        {
            _logger.Fatal($"::EXCEPTION: {e.Message})");
            throw;
        }
    }


    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested) Thread.Sleep(100);

        return Task.CompletedTask;
    }

    private object DeserializeFact(byte[] data)
    {
        return data.FromBytes<TPayload>();
    }


    private Task ConnectAsync(CancellationToken cancellationToken)
    {
        return Task.Run(async () =>
        {
            try
            {
                _bus = _connectionFactory.CreateEncodedConnection(_configureOptions);
                while (_bus.IsClosed())
                {
                    _logMessage = $"CONNECTING NATS [{_bus.ConnectedId}]";
                    Thread.Sleep(2_000);
                }

                _logMessage = $"CONNECTED NATS [{_bus.ConnectedId}]";
                _logger.Information(_logMessage);
                _bus.OnDeserialize += DeserializeFact;
            }
            catch (Exception e)
            {
                _logger.Fatal(e.InnerAndOuter());
                throw;
            }
        }, cancellationToken);
    }

    public override void Dispose()
    {
        _subscription.Dispose();
        _bus.Dispose();
    }
}