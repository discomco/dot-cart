using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;
using Serilog;

namespace DotCart.Drivers.NATS;


public static partial class Inject
{
    public static IServiceCollection AddNATSResponder<THope, TCmd>(this IServiceCollection services) 
        where THope : IHope
        where TCmd : ICmd
    {
        return services
            .AddCoreNATS()
            .AddTransient<INATSResponderDriver<THope>, NATSResponderDriverT<THope>>()
            .AddTransient<INATSResponder<THope, TCmd>, NATSResponder<THope, TCmd>>();
    }
    
    public static IServiceCollection AddSpokedNATSResponder<TSpoke, THope, TCmd>(this IServiceCollection services) 
        where THope : IHope
        where TCmd : ICmd
    {
        return services
            .AddCoreNATS()
            .AddTransient<INATSResponderDriver<THope>, NATSResponderDriverT<THope>>()
            .AddTransient<IActor<TSpoke>, NATSResponder<TSpoke,THope, TCmd>>();
    }
    
}

public interface INATSResponder<THope, TCmd> : IResponderT3<INATSResponderDriver<THope>, THope, TCmd>
    where THope : IHope where TCmd : ICmd {}


public class NATSResponder<TSpoke, THope, TCmd> : NATSResponder<THope, TCmd>, IActor<TSpoke> 
    where THope : IHope where TCmd : ICmd
{
    protected NATSResponder(IExchange exchange,
        INATSResponderDriver<THope> responderDriver,
        ICmdHandler cmdHandler,
        Hope2Cmd<TCmd, THope> hope2Cmd) : base(exchange,
        responderDriver,
        cmdHandler,
        hope2Cmd)
    {
    }
}

public class NATSResponder<THope, TCmd> : 
    ResponderT<INATSResponderDriver<THope>, THope, TCmd>, 
    INATSResponder<THope,TCmd> where THope : IHope where TCmd : ICmd
{
    protected NATSResponder(IExchange exchange,
        INATSResponderDriver<THope> responderDriver,
        ICmdHandler cmdHandler,
        Hope2Cmd<TCmd, THope> hope2Cmd) : base(exchange,
        responderDriver,
        cmdHandler,
        hope2Cmd)
    {
    }
}

public interface INATSResponderDriver<THope> : IResponderDriverT<THope> where THope : IHope {}

public class NATSResponderDriverT<THope> : INATSResponderDriver<THope>
    where THope : IHope
{
    private readonly IEncodedConnection _bus;
    private IActor _actor;
    private CancellationTokenSource _cts;
    private string _logMessage;
    private ISubscription _subscription;

    protected NATSResponderDriverT(IEncodedConnection bus=null)
    {
        _bus = bus;
        _bus.OnDeserialize += OnDeserialize;
        _bus.OnSerialize += OnSerialize;
    }


    public Task StopRespondingAsync(CancellationToken cancellationToken = default)
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

    public void SetActor(IActor actor)
    {
        _actor = actor;
    }

    public Task StartRespondingAsync(CancellationToken cancellationToken)
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
                        var rsp = await _actor.HandleCall(hope, cancellationToken);
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