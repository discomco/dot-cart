using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Actors;
using Serilog;

namespace DotCart.TestFirst.Drivers;

public class EventFeeder<TID, TDoc>
    : ActorB, IEventFeeder
    where TID : IID
    where TDoc : IState
{
    private readonly IEventStore _eventStore;
    private readonly IDCtorT<TID> _newId;
    private readonly StateCtorT<TDoc> _newState;
    private readonly EventStreamGenFunc<TID> _newStream;

    public EventFeeder(
        IExchange exchange,
        IEventStore eventStore,
        EventStreamGenFunc<TID> newStream,
        IDCtorT<TID> newID,
        StateCtorT<TDoc> newState) : base(exchange)
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

public interface IEventFeeder;