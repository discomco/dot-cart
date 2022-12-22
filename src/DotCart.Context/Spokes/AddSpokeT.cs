using DotCart.Abstractions.Actors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DotCart.Context.Spokes;


public static class Inject
{
    public static IServiceCollection AddActorHost(this IServiceCollection services)
    {
        return services
            .AddHostedService<Cartwheel>();
    }
    
    public static IServiceCollection AddHostedSpokeT<TSpoke, TSpokeBuilder>(this IServiceCollection services) 
        where TSpoke: SpokeT<TSpoke>
        where TSpokeBuilder: SpokeBuilderT<TSpoke>
    {
        return services
            .AddTransient<TSpoke>()
            .AddSingleton<ISpokeBuilder<TSpoke>, TSpokeBuilder>()
            .AddHostedService(provider =>
            {
                var spokeBuilder = provider.GetRequiredService<ISpokeBuilder<TSpoke>>();
                return spokeBuilder.Build();
            });

    }

    
    
    
}


