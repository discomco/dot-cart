using DotCart.Behavior;
using DotCart.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestKit;

public static class Inject
{
    public static IServiceCollection AddFakeFactsBus(this IServiceCollection services)
    {
        return services
            .AddSingleton<IFakeFactsBus, FakeFactsBus>();
    }
}

public class FakeFactsBus : TopicPubSub<IFact>, IFakeFactsBus
{
}

public interface IFakeFactsBus : ITopicPubSub<IFact>
{
}