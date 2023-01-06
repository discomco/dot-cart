using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DotCart.Context.Actors;

public static partial class Inject
{
    public static IServiceCollection AddTopicMediator(this IServiceCollection services)
    {
        services.TryAddSingleton<ITopicMediator, TopicMediator>();
        return services;
    }
}

internal sealed class TopicMediator : TopicPubSub<IEvtB>, ITopicMediator
{
}