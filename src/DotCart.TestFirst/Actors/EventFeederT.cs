using System.Reflection;
using System.Reflection.Emit;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Actors;
using DotCart.TestKit.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DotCart.TestFirst.Actors;

public static class Inject
{
    public static IServiceCollection AddEventFeeder<TID, TDoc>(this IServiceCollection services)
        where TDoc : IState
        where TID : IID
    {
        return services
            .AddTransient<IEventFeeder, EventFeederT<TID, TDoc>>();
    }
}


public record StartFeeding : IMsg
{
    private static StartFeeding? _it;

    public static StartFeeding It()
    {
        return _it ??= new StartFeeding();
    }
}
public record StopFeeding : IMsg
{
    private static StopFeeding? _it;

    public static StopFeeding It()
    {
        return _it ??= new StopFeeding();
    }
}


public class EventFeederT<TID, TDoc>(
    IExchange exchange,
    IEventStore eventStore,
    EventStreamGenFuncT<TID> newStream,
    IDCtorT<TID> newID,
    StateCtorT<TDoc> newState)
    : ActorB(exchange), IEventFeeder
    where TID : IID
    where TDoc : IState
{
    private readonly StateCtorT<TDoc> _newState = newState;
    private static CancellationTokenSource? _cts;

    private static CancellationTokenSource Cts => _cts ??= new CancellationTokenSource();

    public override async Task HandleCast(IMsg msg, CancellationToken cancellationToken)
    {
        switch (msg)
        {
            case StartFeeding:

                while (!Cts.Token.IsCancellationRequested)
                {
                    var ID = newID();
                    var events = newStream(ID);
                    Log.Information("Feeding Events");
                    var res = await eventStore.AppendEventsAsync(ID, events, cancellationToken);
                    Thread.Sleep(1_000);
                }
                break;
            case StopFeeding:
                {
                    await Cts.CancelAsync();
                    Log.Information("Stopping Feeding");
                    break;
                }
            default:
                Log.Warning("Unknown message type: {msgType}", msg.GetType());
                break;
        }
    }

    public override Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {

        return (Task<IMsg>)Task.CompletedTask;
    }

    protected override Task CleanupAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected override Task StartActingAsync(CancellationToken cancellationToken)
    {
        Log.Information("Starting EventFeeder");
        return Task.CompletedTask;
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken)
    {
        Log.Information("Stopping EventFeeder");
        return Task.CompletedTask;
    }
}

public interface IEventFeeder
    : IActorB;