using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using NATS.Client;
using Serilog;

namespace DotCart.Drivers.NATS;

public class NATSResponderDriverT<THope> : DriverB, IResponderDriverT<THope>
    where THope : IHopeB
{
    private readonly IEncodedConnection _bus;
    private CancellationTokenSource _cts;
    private string _logMessage;
    private ISubscription _subscription;

    public NATSResponderDriverT(IEncodedConnection bus)
    {
        _bus = bus;
    }


    public override void Dispose()
    {
        if (_subscription != null)
            _subscription.Dispose();
        if (_bus != null)
            _bus.Dispose();
    }

    public Task StartRespondingAsync(CancellationToken cancellationToken = default)
    {
        return Task.Run(async () =>
        {
            try
            {
                await ConnectAsync(cancellationToken);
                _logMessage = $"NATS{AppVerbs.Responding} Topic: [{TopicAtt.Get<THope>()}] on bus [{_bus.ConnectedId}]";
                Log.Debug(_logMessage);
                _logMessage = "";
                _subscription = _bus.SubscribeAsync(
                    TopicAtt.Get<THope>(),
                    async (sender, args) =>
                    {
                        var hope = (THope)args.ReceivedObject;
                        Log.Debug($"{AppFacts.Received} NATS.Req {args.Subject} ~> {hope.AggId}");
                        var msg = await Call(hope, cancellationToken);
                        var rsp = (Feedback)msg;
                        args.Message.Respond(rsp.ToBytes());
                        Log.Debug($"NATS{AppFacts.Responded} NATS.Rsp {args.Reply} ~> Id: {rsp.AggId}, {rsp.IsSuccess} ");
                    });
            }
            catch (Exception e)
            {
                _logMessage = $"{e.InnerAndOuter().AsError()}";
                Log.Fatal(_logMessage);
                throw;
            }
        }, cancellationToken);
    }

    public Task StopRespondingAsync(CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            if (_subscription.PendingMessages > 0)
                _subscription.Drain();
            _subscription.Unsubscribe();
            _bus.OnDeserialize -= OnDeserialize;
            _bus.OnSerialize -= OnSerialize;
        }, cancellationToken);
    }


    private byte[] OnSerialize(object obj)
    {
        return obj.ToBytes();
    }

    private object OnDeserialize(byte[] data)
    {
        return data.FromBytes<THope>();
    }

    private Task ConnectAsync(CancellationToken cancellationToken)
    {
        return Task.Run(async () =>
        {
            while (_bus.IsClosed())
            {
                var connecting = "CONNECTING".AsVerb();
                _logMessage = $"{connecting} NATS [{_bus.ConnectedId}]";
                Thread.Sleep(1_000);
            }

            var connected = "CONNECTED".AsFact();
            _logMessage = $"{connected} NATS [{_bus.ConnectedId}]";
            Log.Information(_logMessage);
            _bus.OnDeserialize += OnDeserialize;
            _bus.OnSerialize += OnSerialize;
        }, cancellationToken);
    }
}