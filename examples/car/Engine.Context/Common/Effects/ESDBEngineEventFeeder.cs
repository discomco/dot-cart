using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using Engine.Context.Initialize;
using Engine.Contract.Schema;
using Serilog;

namespace Engine.Context.Common.Effects;

public class ESDBEngineEventFeeder : ActorB, IESDBEngineEventFeeder, IProducer
{
    private readonly IEventStoreDriver _eventStore;
    private readonly NewID<EngineID> _newId;
    private readonly NewState<Schema.Engine> _newState;
    private readonly EventStreamGenerator<EngineID> _newStream;

    public ESDBEngineEventFeeder(
        IExchange exchange,
        IEventStoreDriver eventStore,
        EventStreamGenerator<EngineID> newStream,
        NewID<EngineID> newID,
        NewState<Schema.Engine> newState) : base(exchange)
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

public interface IESDBEngineEventFeeder : IActor<Spoke>
{
}