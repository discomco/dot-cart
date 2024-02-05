using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using NATS.Client;
using Serilog;

namespace DotCart.Drivers.NATS;

public class NATSListenerDriverT<TPayload, TMeta>
    : DriverB, INATSListenerDriverT<TPayload>
    where TPayload : IPayload
    where TMeta : IMetaB
{
    private readonly Action<Options> _configureOptions;
    private readonly INatsClientConnectionFactory _connectionFactory;
    private readonly ILogger _logger;
    private IEncodedConnection? _bus;
    private string _logMessage;
    private IAsyncSubscription? _subscription;

    public NATSListenerDriverT(
        INatsClientConnectionFactory connectionFactory,
        ILogger logger,
        Action<Options> configureOptions)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _configureOptions = configureOptions;
    }


    public string Topic { get; } = FactTopicAtt.Get<TPayload>();

    public async Task StartListeningAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await ConnectAsync(cancellationToken)
                .ConfigureAwait(false);
            _logger.Information($"::SUB(NATS):: [{FactTopicAtt.Get<TPayload>()}] <~ [{_bus.ConnectedId}]");
            _subscription = _bus.SubscribeAsync(
                FactTopicAtt.Get<TPayload>(),
                (_, args) =>
                {
                    var fact = (FactT<TPayload, TMeta>)args.ReceivedObject;
                    try
                    {
                        _logger.Information($"::RCV(NATS): {fact}");
                        Cast(fact, cancellationToken);
                    }
                    catch (Exception e)
                    {
                        _logger.Error($"::ERROR: {e.InnerAndOuter()}");
                    }
                });
        }
        catch (Exception e)
        {
            _logger.Fatal($"::EXCEPTION: {e.Message})");
            throw;
        }
    }

    public Task StopListeningAsync(CancellationToken cancellationToken = default)
    {
        _bus.OnDeserialize -= DeserializeFact;
        _bus.Close();
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _subscription?.Dispose();
        _bus?.Dispose();
    }


    private object DeserializeFact(byte[] data)
    {
        return data.FromBytes<FactT<TPayload, TMeta>>();
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
                    _logMessage = $"::C?(NATS):: [{_bus.ConnectedId}]";
                    Thread.Sleep(2_000);
                }

                _logMessage = $"::C!(NATS):: [{_bus.ConnectedId}]";
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
}

public interface INATSListenerDriverT<TFactPayload>
    : IListenerDriverT<TFactPayload, byte[]>
    where TFactPayload : IPayload
{
}