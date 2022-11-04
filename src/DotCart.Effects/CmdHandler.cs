using Ardalis.GuardClauses;
using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Effects.Drivers;
using DotCart.Schema;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DotCart.Effects;

public interface ICmdHandler
{
    Task<IFeedback> Handle(ICmd cmd);
}
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

    public async Task<IFeedback> Handle(ICmd cmd)
    {
        var fbk = Feedback.Empty;
        try
        {
            Guard.Against.Null(cmd);
            var aggId = cmd.GetID();
            _aggregate.SetID(aggId);
            await _aggregateStoreDriver.LoadAsync(_aggregate).ConfigureAwait(false);
            fbk = await _aggregate.ExecuteAsync(cmd).ConfigureAwait(false);
            await _aggregateStoreDriver.SaveAsync(_aggregate).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            fbk.SetError(e.AsError());
            Log.Error(e.Message);
        }
        return fbk;
    }

    public void Dispose()
    {
        _aggregateStoreDriver.Dispose();
    }
}