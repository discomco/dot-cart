using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using NATS.Client;
using Serilog;

namespace DotCart.Drivers.NATS;

public abstract class NATSRequesterDriverT : IRequesterDriverB
{
    private readonly IEncodedConnection _bus;

    protected NATSRequesterDriverT(IEncodedConnection bus)
    {
        _bus = bus;
        // _bus.OnDeserialize += OnDeserialize;
        // _bus.OnSerialize += OnSerialize;
    }

    // private byte[] OnSerialize(object obj)
    // {
    //     return obj.ToBytes();
    // }
    //
    // private object OnDeserialize(byte[] data)
    // {
    //     return data.FromBytes<THope>();
    // }

    public void Dispose()
    {
        if (_bus == null) return;
        //_bus.OnSerialize -= OnSerialize;
        // _bus.OnDeserialize -= OnDeserialize;
        _bus.Dispose();
    }

    public void SetActor(IActor actor)
    {
    }

    public async Task<Feedback> RequestAsync<THope>(THope hope, CancellationToken cancellationToken = default)
        where THope : IHope
    {
        var res = Feedback.New(hope.AggId);
        try
        {
            if (!_bus.IsClosed()) Log.Debug($"::CONNECT bus: {_bus.ConnectedId}");
            Log.Debug($"::REQUEST hope: AggId:{hope.AggId} on topic {hope.Topic}.");
            var obj = _bus.Request(hope.Topic, hope.ToBytes());

            var bytes = obj.ToBytes();
            res = bytes.FromBytes<Feedback>();
            Log.Debug($"::RECEIVED feedback: AggId:{res.AggId} on {res.Topic}.");
        }
        catch (Exception e)
        {
            res.SetError(e.AsError());
            Log.Fatal($":: REQUEST :: [{GetType()}] = {e.InnerAndOuter()}");
        }

        return res;
    }
}