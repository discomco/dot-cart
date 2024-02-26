using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Core;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using NATS.Client;
using Serilog;

namespace DotCart.Drivers.NATS;

public class NATSEmitter<TPayload, TMeta>
    : INATSEmitter<TPayload, TMeta>, IDisposable
    where TPayload : IPayload
    where TMeta : IMetaB
{
    private readonly IEncodedConnection _bus;
    private readonly ILogger _logger;

    public NATSEmitter(ILogger logger,
        INatsClientConnectionFactory connectionFactory,
        Action<Options> configureOptions)
    {
        _logger = logger;
        _bus = connectionFactory.CreateEncodedConnection(configureOptions);
        _bus.OnSerialize += SerializeRequest;
        _bus.OnDeserialize += DeserializeResponse;
    }

    public void Dispose()
    {
        if (_bus == null) return;

        _bus.OnSerialize -= SerializeRequest;
        _bus.OnDeserialize -= DeserializeResponse;
        _bus.Dispose();
    }


    public Task EmitAsync(FactT<TPayload, TMeta> fact)
    {
        return Task.Run(() =>
        {
            try
            {
                if (!_bus.IsClosed()) _logger.Debug($"::C!(NATS):: {_bus.ConnectedId}");

                _logger.Debug($"::PUB(NATS):: [{fact.Payload}] ~> [{FactTopicAtt.Get<TPayload>()}].");
                _bus.Publish(FactTopicAtt.Get<TPayload>(), fact);
            }
            catch (Exception e)
            {
                _logger.Fatal($"::ERROR:: [{GetType()}] = {e.InnerAndOuter()}");
            }
        });
    }

    private byte[] SerializeRequest(object obj)
    {
        return obj.ToBytes();
    }

    private object DeserializeResponse(byte[] data)
    {
        var rsp = data.FromBytes<Feedback>();
        return rsp;
    }
}

public interface INATSEmitter<TPayload, TMeta>
    where TPayload : IPayload
    where TMeta : IMetaB
{
    Task EmitAsync(FactT<TPayload, TMeta> fact);
}