using DotCart.Abstractions;
using DotCart.Abstractions.Behavior;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Behavior;

public static partial class Inject
{
    private static readonly MetaCtorT<MetaB>
        _newMeta =
            id => MetaB.New(NameAtt.Get<IEngineAggregateInfo>(), id);

    public static IServiceCollection AddMetaCtor(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _newMeta);
    }

    public static IServiceCollection AddEngineBehavior(this IServiceCollection services)
    {
        return services
            .AddInitializeBehavior()
            .AddStartBehavior()
            .AddChangeRpmBehavior()
            .AddStopBehavior()
            .AddChangeDetailsBehavior();
    }
}