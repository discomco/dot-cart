using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Domain.Tests.Engine;

public static partial class Inject
{
    public static IServiceCollection AddEngineAggregate(this IServiceCollection services)
    {
        return services
            .AddTopicPubSub()
            .AddSingleton(Schema.Tests.Engine.New)
            .AddTransient<IEngineAggregate, EngineAggregate>()
            .AddStartOnInitializedPolicy();
    }
}