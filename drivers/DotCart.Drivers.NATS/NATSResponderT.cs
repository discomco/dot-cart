using System.Text.Json;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Core;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Microsoft.Extensions.Hosting;
using NATS.Client;
using Serilog;

namespace DotCart.Drivers.NATS;

public class NATSResponderT<TPayload>
    : BackgroundService
    where TPayload : IPayload
{
    private readonly Action<Options> _configureOptions;
    private readonly INatsClientConnectionFactory _connectionFactory;
    private readonly ILogger _logger;
    private readonly ProcessHopeAsync<TPayload> _processor;
    private IEncodedConnection? _bus;
    private string? _logMessage;
    private IAsyncSubscription? _subscription;

    public NATSResponderT(
        INatsClientConnectionFactory connectionFactory,
        Action<Options> configureOptions,
        ILogger logger,
        ProcessHopeAsync<TPayload> processor)
    {
        _connectionFactory = connectionFactory;
        _configureOptions = configureOptions;
        _processor = processor;
        _logger = logger;
    }

    private byte[] SerializeResponse(object obj)
    {
        return obj.ToBytes();
    }

    private object? DeserializeRequest(byte[] data)
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
                _bus.OnDeserialize += DeserializeRequest;
                _bus.OnSerialize += SerializeResponse;
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


    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            await ConnectAsync(cancellationToken).ConfigureAwait(false);
            _logMessage =
                $"[{HopeTopicAtt.Get<TPayload>()}]-RSP on [{JsonSerializer.Serialize(_bus.DiscoveredServers)}]";
            _logger.Debug(_logMessage);
            _logMessage = "";
            _subscription = _bus.SubscribeAsync(
                HopeTopicAtt.Get<TPayload>(),
                async (_, args) =>
                {
                    if (args.ReceivedObject is not IHopeT<TPayload> req) return;

                    _logger.Debug($"[{HopeTopicAtt.Get<TPayload>()}]-Request {req}");

                    var fbk = await _processor(req)
                        .ConfigureAwait(false);
                    args.Message.Respond(fbk.ToBytes());
                    //                    _bus.Publish(args.Message.Reply, fbk);
                    _bus.Flush();
                    _logger.Debug($"[{HopeTopicAtt.Get<TPayload>()}]-FEEDBACK {fbk.ErrState.IsSuccessful} ");
                });
        }
        catch (Exception e)
        {
            _logMessage = $"[{HopeTopicAtt.Get<TPayload>()}]-ERR {JsonSerializer.Serialize(e.AsError())}";
            _logger.Fatal(_logMessage);
            _subscription.DrainAsync();
        }
    }


    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested) Thread.Sleep(1_000);

        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            _logger.Information($"Gracefully shutting down {GetType()}");
            if (_subscription.PendingMessages > 0) _subscription.Drain();

            _subscription.Unsubscribe();
            _bus.OnDeserialize -= DeserializeRequest;
            _bus.OnSerialize -= SerializeResponse;
        }, cancellationToken);
    }
}