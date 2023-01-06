using Ardalis.GuardClauses;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behavior;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DotCart.Context.Actors;

public static partial class Inject
{
    public static IServiceCollection AddCmdHandler(this IServiceCollection services)
    {
        return services
            .AddTransient<ICmdHandler, CmdHandler>();
    }
}

internal class CmdHandler : ICmdHandler
{
    private readonly IAggregate _aggregate;
    private readonly IAggregateStore _aggregateStore;

    public CmdHandler(
        IAggregateBuilder aggBuilder,
        IAggregateStore aggregateStore)
    {
        _aggregate = aggBuilder.Build();
        _aggregateStore = aggregateStore;
    }

    public async Task<Feedback> HandleAsync(ICmdB cmd, Feedback previous, CancellationToken cancellationToken = default)
    {
        var step = "unknown";
        if (previous != null)
            step = previous.Step;
        var fbk = Feedback.New(cmd.AggregateID.Id(), previous, step);
        try
        {
            Guard.Against.Null(cmd);
            var aggId = cmd.AggregateID;
            _aggregate.SetID(aggId);
            await _aggregateStore
                .LoadAsync(_aggregate, cancellationToken)
                .ConfigureAwait(false);

            fbk = await _aggregate
                .ExecuteAsync(cmd, fbk)
                .ConfigureAwait(false);

            await _aggregateStore
                .SaveAsync(_aggregate, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception e)
        {
            fbk.SetError(e.AsError());
            Log.Error(e.Message);
        }

        return fbk;
    }

    // public void Dispose()
    // {
    //     _aggregateStoreDriver.Dispose();
    // }
}