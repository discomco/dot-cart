using Ardalis.GuardClauses;
using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Effects;

public static class Inject
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
        IAggregate aggregate,
        IAggregateStore aggregateStore)
    {
        _aggregate = aggregate;
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
            _aggregateStore.Load(_aggregate);
            fbk = await _aggregate.ExecuteAsync(cmd);
            _aggregateStore.Save(_aggregate);
        }
        catch (Exception e)
        {
            fbk.SetError(e.AsError());
            Console.WriteLine(e);
        }

        return fbk;
    }
}

public interface ICmdHandler
{
    Task<IFeedback> Handle(ICmd cmd);
}