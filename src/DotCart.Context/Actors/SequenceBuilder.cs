using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Context.Actors;

public static partial class Inject
{
    public static IServiceCollection AddSingletonSequenceBuilder<TAggregateInfo, TAggregateState>(
        this IServiceCollection services)
        where TAggregateInfo : IAggregateInfoB
        where TAggregateState : IState
    {
        return
            services
                .AddSingletonAggregateBuilder<TAggregateInfo, TAggregateState>()
                .AddSingleton<ISequenceBuilder, SequenceBuilder>();
    }
}

public class SequenceBuilder : ISequenceBuilder
{
    private readonly IAggregateBuilder _aggBuilder;
    private readonly IAggregateStore _aggStore;

    public SequenceBuilder(IAggregateBuilder aggBuilder, IAggregateStore aggStore)
    {
        _aggBuilder = aggBuilder;
        _aggStore = aggStore;
    }

    public ICmdHandler Build()
    {
        return new CmdHandler(_aggBuilder, _aggStore);
    }
}