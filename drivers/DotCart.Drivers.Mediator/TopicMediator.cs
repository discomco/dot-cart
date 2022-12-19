using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Drivers.Mediator;

public static partial class Inject
{
    public static IServiceCollection AddTopicMediator(this IServiceCollection services)
    {
        return services
            .AddSingleton<ITopicMediator, TopicMediator>();
    }
}

internal sealed class TopicMediator : TopicPubSub<IEvtB>, ITopicMediator
{
}