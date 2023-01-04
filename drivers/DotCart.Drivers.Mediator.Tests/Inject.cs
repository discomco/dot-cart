using DotCart.Abstractions.Actors;
using DotCart.Context.Actors;
using DotCart.Context.Spokes;
using DotCart.Drivers.InMem;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Drivers.Mediator.Tests;

public static class Inject
{
    public static IServiceCollection AddTestEnv(this IServiceCollection services)
    {
        return services
            .AddSingletonExchange()
            .AddMemProjector()
            .AddTransient<TheSpoke>()
            .AddTransient<ISpokeBuilder<TheSpoke>, TheSpokeBuilder>()
            .AddTransient<IActorT<TheSpoke>, Consumer2>()
            .AddTransient<IConsumer2, Consumer2>()
            .AddTransient<IActorT<TheSpoke>, Consumer1>()
            .AddTransient<IConsumer1, Consumer1>()
            .AddTransient<IActorT<TheSpoke>, Producer>()
            .AddTransient<IProducer, Producer>()
            .AddTransient<INamedConsumer, NamedConsumer1>()
            .AddTransient<IActorT<TheSpoke>, NamedConsumer1>()
            .AddTransient<INamedConsumer, NamedConsumer2>()
            .AddTransient<IActorT<TheSpoke>, NamedConsumer2>()

            // We add some duplicate consumers
            .AddTransient<INamedConsumer, NamedConsumer1>()
            .AddTransient<IActorT<TheSpoke>, NamedConsumer1>()
            .AddTransient<INamedConsumer, NamedConsumer2>()
            .AddTransient<IActorT<TheSpoke>, NamedConsumer2>()
            .AddHostedService(provider =>
            {
                var builder = provider.GetRequiredService<ISpokeBuilder<TheSpoke>>();
                return builder.Build();
            });
    }
}