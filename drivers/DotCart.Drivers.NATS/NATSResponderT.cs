using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using NATS.Client;
using Serilog;

namespace DotCart.Drivers.NATS;

public class NATSResponderT<TSpoke, THope, TCmd> : NATSResponderT<THope, TCmd>, IActor<TSpoke>
    where TSpoke : ISpokeB
    where THope : IHope
    where TCmd : ICmd
{
    public NATSResponderT(IEncodedConnection bus,
        IExchange exchange,
        ICmdHandler cmdHandler,
        Hope2Cmd<TCmd, THope> hope2Cmd) : base(bus,
        exchange,
        cmdHandler,
        hope2Cmd)
    {
    }
}

public class NATSResponderT<THope, TCmd> : ResponderT<THope, TCmd>
    where THope :
    IHope
    where TCmd : ICmd
{
    private readonly IEncodedConnection _bus;
    private CancellationTokenSource _cts;
    private string _logMessage;
    private ISubscription _subscription;

    public NATSResponderT(
        IEncodedConnection bus,
        IExchange exchange,
        ICmdHandler cmdHandler,
        Hope2Cmd<TCmd, THope> hope2Cmd) : base(exchange,
        cmdHandler,
        hope2Cmd)
    {
        _bus = bus;
        _bus.OnDeserialize += OnDeserialize;
        _bus.OnSerialize += OnSerialize;
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            if (_subscription.PendingMessages > 0) _subscription.Drain();

            _subscription.Unsubscribe();
        }, cancellationToken);
    }

    // private async Task StopResondingAsync(CancellationToken cancellationToken)
    // {
    //     if (_bus.IsConnected)
    //     {
    //         _logMessage = $"::UNSUBSCRIBING ::Topic: [{CommandTopic}]";
    //         _logger?.Debug(_logMessage);
    //         await _bus.UnsubAsync(_subscription);
    //         _logMessage = $"::DISCONNECTING ::Bus: [{_bus.Id}]";
    //         _logger?.Debug(_logMessage);
    //         _bus.Disconnect();
    //     };
    // }

    public void Dispose()
    {
        if (_subscription != null) _subscription.Dispose();

        if (_bus == null) return;
        _bus.OnDeserialize -= OnDeserialize;
        _bus.OnSerialize -= OnSerialize;
    }

    protected override Task StartActingAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            try
            {
                if (!_bus.IsClosed())
                {
                    _logMessage = $"::CONNECT ::Bus [{_bus.ConnectedId}]";
                    Log.Information(_logMessage);
                }

                _logMessage = $"::RESPOND ::Topic: [{TopicAtt.Get<THope>()}] on bus [{_bus.ConnectedId}]";
                Log.Debug(_logMessage);
                _logMessage = "";
                _subscription = _bus.SubscribeAsync(
                    TopicAtt.Get<THope>(),
                    async (sender, args) =>
                    {
                        var hope = args.Message.Data.FromBytes<THope>();
                        Log.Debug($"::RECEIVED:: {args.Subject}");
                        var rsp = await HandleCall(hope, cancellationToken);
                        _bus.Publish(args.Reply, rsp.ToBytes());
                        Log.Debug($"::RESPONDED:: {args.Reply} ~> {rsp.Topic} ");
                    });
            }
            catch (Exception e)
            {
                _logMessage = $"::EXCEPTION {e.Message}";
                Log.Fatal(_logMessage);
                throw;
            }
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
}