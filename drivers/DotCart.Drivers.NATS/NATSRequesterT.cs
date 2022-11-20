using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Clients;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using NATS.Client;
using Serilog;

namespace DotCart.Drivers.NATS;

public class NATSRequesterT<THope> : RequesterT<THope> where THope : IHope
{
    private readonly IEncodedConnection _bus;

    protected NATSRequesterT(IEncodedConnection bus)
    {
        _bus = bus;
    }


    private byte[] OnSerialize(object obj)
    {
        return obj.ToBytes();
    }
    
    private object OnDeserialize(byte[] data)
    {
        return data.FromBytes<THope>();
    }

    public override void Dispose()
    {
        if (_bus == null) return;
        _bus.OnSerialize -= OnSerialize;
        _bus.OnDeserialize -= OnDeserialize;
        _bus.Dispose();
    }

    public override async Task<Feedback> RequestAsync(THope hope, CancellationToken cancellationToken = default)
    {
        var res = Feedback.New(hope.AggId);
        try
        {
            if (!_bus.IsClosed()) 
                Log.Debug($"::CONNECT bus: {_bus.ConnectedId}");
            Log.Debug($"::REQUEST hope: AggId:{hope.AggId} on topic {hope.Topic}.");
            res = (Feedback)_bus.Request(hope.Topic, hope);
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