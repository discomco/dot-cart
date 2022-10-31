using Ardalis.GuardClauses;
using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Schema;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DotCart.Effects;

public static class Inject
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
    private readonly IAggregateStore _aggregateStore;

    public CmdHandler(
        IAggregateBuilder aggBuilder,
        IAggregateStore aggregateStore)
    {
        _aggregate = aggBuilder.Build();
        _aggregateStore = aggregateStore;
    }

    public async Task<IFeedback> Handle(ICmd cmd)
    {
        var fbk = Feedback.Empty;
        try
        {
            Guard.Against.Null(cmd);
            var aggId = cmd.GetID();
            _aggregate.SetID(aggId);
            _aggregateStore.LoadAsync(_aggregate);
            fbk = await _aggregate.ExecuteAsync(cmd);
            _aggregateStore.SaveAsync(_aggregate);
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
    }
}