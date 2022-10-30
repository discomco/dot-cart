using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Effects;
using Microsoft.Extensions.DependencyInjection;


namespace DotCart.TestKit;

public static partial class Inject
{
    public static IServiceCollection AddFakeAsyncInfra(this IServiceCollection services)
    {
        return services
            .AddSingleton<IFakeAsyncInfra, FakeAsyncInfra>();
    }
}

public class FakeAsyncInfra : TopicPubSub<IFact>, IFakeAsyncInfra
{
}

public interface IFakeAsyncInfra: ITopicPubSub<IFact>
{
}