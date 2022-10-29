using DotCart.Behavior;
using DotCart.TestEnv.Engine.Behavior;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestEnv.Engine;

public static class Inject
{
    public static IServiceCollection AddEngineAggregate(this IServiceCollection services)
    {
        return services
            .AddTopicPubSub()
            .AddSingleton(Schema.Engine.Ctor)
            .AddTransient<IEngineAggregate, EngineAggregate>()
            .AddTransient<IAggregate, EngineAggregate>()
            .AddStartOnInitializedPolicy();
    }
}