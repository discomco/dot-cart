using DotCart.Contract;
using DotCart.Effects;
using DotCart.Effects.Drivers;
using DotCart.Schema;
using DotCart.TestEnv.Engine.Schema;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DotCart.TestEnv.Engine.Effects;

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
    private readonly ILogger _logger;
    private readonly IEventStoreDriver _eventStore;
    private readonly EventStreamGenerator<EngineID, Schema.Engine> _newStream;
    private readonly NewID<EngineID> _newId;
    private readonly NewState<Schema.Engine> _newState;

    public ESDBEngineEventFeeder(
        ILogger logger,
        IEventStoreDriver eventStore,
        EventStreamGenerator<EngineID, Schema.Engine> newStream,
        NewID<EngineID> newID,
        NewState<Schema.Engine> newState)
    {
        _logger = logger;
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
        _logger.Information("Starting EventFeeder");
        while (!cancellationToken.IsCancellationRequested)
        {
            var ID = _newId();
            var events = _newStream(ID, _newState);
            Thread.Sleep(10_000);
            _logger.Information($"Feeding Events");
            await _eventStore.AppendEventsAsync(ID, events, cancellationToken);
        }
    }

    protected override Task StopReactingAsync(CancellationToken cancellationToken)
    {
        _logger.Information("Stopping EventFeeder");
        return Task.CompletedTask;
    }
}

public interface IESDBEngineEventFeeder : IReactor
{
}