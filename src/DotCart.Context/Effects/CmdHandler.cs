using Ardalis.GuardClauses;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DotCart.Context.Effects;

public static partial class Inject
{
    public static IServiceCollection AddCmdHandler(this IServiceCollection services)
    {
        return services
            .AddAggregateBuilder()
            .AddTransient<ICmdHandler, CmdHandler>();
    }
}

internal class CmdHandler : ICmdHandler
{
    private readonly IAggregate _aggregate;
    private readonly IAggregateStoreDriver _aggregateStoreDriver;

    public CmdHandler(
        IAggregateBuilder aggBuilder,
        IAggregateStoreDriver aggregateStoreDriver)
    {
        _aggregate = aggBuilder.Build();
        _aggregateStoreDriver = aggregateStoreDriver;
    }

    public async Task<Feedback> HandleAsync(ICmd cmd, CancellationToken cancellationToken = default)
    {
        var fbk = Feedback.Empty;
        try
        {
            Guard.Against.Null(cmd);
            var aggId = cmd.AggregateID;

            _aggregate.SetID(aggId);

            await _aggregateStoreDriver
                .LoadAsync(_aggregate, cancellationToken)
                .ConfigureAwait(false);

            fbk = await _aggregate
                .ExecuteAsync(cmd)
                .ConfigureAwait(false);

            await _aggregateStoreDriver
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