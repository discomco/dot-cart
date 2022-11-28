using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using Engine.Contract;
using Serilog;

namespace Engine.Utils;

public class EventFeeder : ActorB, IEventFeeder, IProducer
{
    private readonly IEventStore _eventStore;
    private readonly IDCtorT<Contract.Schema.EngineID> _newId;
    private readonly StateCtorT<Behavior.Engine> _newState;
    private readonly EventStreamGenerator<Contract.Schema.EngineID> _newStream;

    public EventFeeder(
        IExchange exchange,
        IEventStore eventStore,
        EventStreamGenerator<Contract.Schema.EngineID> newStream,
        IDCtorT<Contract.Schema.EngineID> newID,
        StateCtorT<Behavior.Engine> newState) : base(exchange)
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
            Log.Information("Feeding Events");
            await _eventStore.AppendEventsAsync(ID, events, cancellationToken);
            Thread.Sleep(60_000);
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