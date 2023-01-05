using DotCart.Abstractions.Clients;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using NATS.Client;
using Serilog;

namespace DotCart.Drivers.NATS;

public class NATSRequesterT<TPayload> : RequesterT<TPayload> where TPayload : IPayload
{
    private readonly Action<Options> _configureOptions;
    private readonly INatsClientConnectionFactory _connectionFactory;
    private IEncodedConnection _bus;

    protected NATSRequesterT(INatsClientConnectionFactory connectionFactory, Action<Options> configureOptions)
    {
        _connectionFactory = connectionFactory;
        _configureOptions = configureOptions;
    }


    private byte[] OnSerialize(object obj)
    {
        return obj.ToBytes();
    }

    private object OnDeserialize(byte[] data)
    {
        return data.FromBytes<HopeT<TPayload>>();
    }

    public override void Dispose()
    {
        if (_bus == null)
            return;
        _bus.OnSerialize -= OnSerialize;
        _bus.OnDeserialize -= OnDeserialize;
        _bus.Dispose();
    }

    public override async Task<Feedback> RequestAsync(HopeT<TPayload> hope,
        CancellationToken cancellationToken = default)
    {
        var res = Feedback.New(hope.AggId);
        try
        {
            _bus = _connectionFactory.CreateEncodedConnection(_configureOptions);
            if (!_bus.IsClosed())
                Log.Debug($"::CONNECT bus: {_bus.ConnectedId}");
            Log.Debug($"::REQUEST hope: AggId:{hope.AggId} on topic {TopicAtt.Get(hope)}.");
            res = (Feedback)_bus.Request(TopicAtt.Get(hope), hope);
            Log.Debug($"::RECEIVED feedback: AggId:{res.AggId}.");
        }
        catch (Exception e)
        {
            res.SetError(e.AsError());
            Log.Fatal($":: REQUEST :: [{GetType()}] = {e.InnerAndOuter()}");
        }

        return res;
    }
}