using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using Engine.Contract;
using Serilog;

namespace Engine.Utils;

public class EventFeeder : ActorB, IEventFeeder, IProducer
{
    private readonly IEventStore _eventStore;
    private readonly IDCtor<Schema.EngineID> _newId;
    private readonly StateCtor<Behavior.Engine> _newState;
    private readonly EventStreamGenerator<Schema.EngineID> _newStream;

    public EventFeeder(
        IExchange exchange,
        IEventStore eventStore,
        EventStreamGenerator<Schema.EngineID> newStream,
        IDCtor<Schema.EngineID> newID,
        StateCtor<Behavior.Engine> newState) : base(exchange)
    {
        _eventStore = eventStore;
        _newStream = newStream;
        _newId = newID;
        _newState = newState;
    }


    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public override Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {
        return (Task<IMsg>)Task.CompletedTask;
    }

    protected override Task CleanupAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected override async Task StartActingAsync(CancellationToken cancellationToken)
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

    protected override Task StopActingAsync(CancellationToken cancellationToken)
    {
        Log.Information("Stopping EventFeeder");
        return Task.CompletedTask;
    }
}

public interface IEventFeeder
{
}