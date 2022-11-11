using DotCart.Context.Effects;
using DotCart.Context.Effects.Drivers;
using DotCart.Context.Schemas;
using DotCart.Contract.Dtos;
using DotCart.Contract.Schemas;
using Engine.Context.Common.Schema;
using Engine.Contract.Schema;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Engine.Context.Common.Effects;

public static class Inject
{
    public static IServiceCollection AddESDBEngineEventFeeder(this IServiceCollection services)
    {
        return services
            .AddEngineCtor()
            .AddHostedService<ESDBEngineEventFeeder>();
    }
}

public class ESDBEngineEventFeeder : Reactor, IESDBEngineEventFeeder
{
    private readonly IEventStoreDriver _eventStore;
    private readonly NewID<EngineID> _newId;
    private readonly NewState<Schema.Engine> _newState;
    private readonly EventStreamGenerator<EngineID> _newStream;

    public ESDBEngineEventFeeder(
        IEventStoreDriver eventStore,
        EventStreamGenerator<EngineID> newStream,
        NewID<EngineID> newID,
        NewState<Schema.Engine> newState)
    {
        _eventStore = eventStore;
        _newStream = newStream;
        _newId = newID;
        _newState = newState;
    }


    public override Task HandleAsync(IMsg msg, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    protected override async Task StartReactingAsync(CancellationToken cancellationToken)
    {
        Log.Information("Starting EventFeeder");
        while (!cancellationToken.IsCancellationRequested)
        {
            var ID = _newId();
            var events = _newStream(ID);
            Thread.Sleep(10_000);
            Log.Information("Feeding Events");
            await _eventStore.AppendEventsAsync(ID, events, cancellationToken);
        }
    }

    protected override Task StopReactingAsync(CancellationToken cancellationToken)
    {
        Log.Information("Stopping EventFeeder");
        return Task.CompletedTask;
    }
}

public interface IESDBEngineEventFeeder : IReactor
{
}