using DotCart.Behavior;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestEnv.Engine;

public static partial class Inject
{
    public static IServiceCollection AddEngineAggregate(this IServiceCollection services)
    {
        return services
            .AddTopicMediator()
            .AddSingleton(Schema.Engine.Ctor)
            .AddTransient<IAggregate, Aggregate>();
    }
}