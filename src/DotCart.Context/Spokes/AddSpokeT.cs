using DotCart.Abstractions.Actors;
using DotCart.Context.Actors;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Context.Spokes;

public static class Inject
{
    public static IServiceCollection AddActorHost(this IServiceCollection services)
    {
        return services
            .AddHostedService<Cartwheel>();
    }

    public static IServiceCollection AddHostedSpokeT<TSpoke>(this IServiceCollection services)
        where TSpoke : SpokeT<TSpoke>
    {
        return services
            .AddSingletonExchange()
            .AddTransient<TSpoke>()
            .AddSingleton<ISpokeBuilder<TSpoke>, SpokeBuilderT<TSpoke>>()
            .AddHostedService(provider =>
            {
                var spokeBuilder = provider.GetRequiredService<ISpokeBuilder<TSpoke>>();
                return spokeBuilder.Build();
            });
    }
}