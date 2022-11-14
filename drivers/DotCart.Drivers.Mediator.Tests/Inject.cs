using DotCart.Context.Abstractions;
using DotCart.Context.Spokes;
using DotCart.Drivers.InMem;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Drivers.Mediator.Tests;

public static class Inject
{
    public static IServiceCollection AddTestEnv(this IServiceCollection services)
    {
        return services
            .AddExchange()
            .AddMemProjector()
            .AddTransient<ISpokeT<Spoke>, Spoke>()
            .AddTransient<ISpokeBuilder<ISpokeT<Spoke>>, SpokeBuilder>()
            .AddTransient<IActor<Spoke, Consumer2>, Consumer2>()
            .AddTransient<IActor<ISpokeT<Spoke>>, Consumer2>()
            .AddTransient<IActor<Spoke, Consumer1>, Consumer1>()
            .AddTransient<IActor<ISpokeT<Spoke>>, Consumer1>()
            .AddTransient<IActor<ISpokeT<Spoke>>, Producer>()
            .AddTransient<IActor<Spoke, Producer>, Producer>()
            .AddHostedService(provider =>
            {
                var builder = provider.GetRequiredService<ISpokeBuilder<ISpokeT<Spoke>>>();
                return (Spoke)builder.Build();
            });
    }
}