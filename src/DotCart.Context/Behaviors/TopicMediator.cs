using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Context.Behaviors;

public static partial class Inject
{
    public static IServiceCollection AddTopicMediator(this IServiceCollection services)
    {
        return services
            .AddSingleton<ITopicMediator, TopicMediator>();
    }
}

public interface ITopicMediator : ITopicPubSub<IEvt>, IDisposable
{
}

internal sealed class TopicMediator : TopicPubSub<IEvt>, ITopicMediator
{
}