using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;

namespace DotCart.Polly;

public static class Inject
{
    public static IServiceCollection AddPolly(this IServiceCollection services)
    {
        return services.AddTransient(_ => Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(Config.MaxRetries,
                times => TimeSpan.FromMilliseconds(times * 100)));
    }

    public static IServiceCollection AddRetry<TException>(this IServiceCollection services,
        int retryCount,
        Action<Exception, int>? onRetry = null)
        where TException : Exception
    {
        if (onRetry == null)
            return services
                .AddTransient<RetryPolicy>(_ => Policy
                    .Handle<TException>()
                    .Retry(retryCount)
                );

        return services
            .AddTransient<RetryPolicy>(_ => Policy
                .Handle<TException>()
                .Retry(retryCount, onRetry)
            );
    }

    public static IServiceCollection AddWaitAndRetry<TException>(this IServiceCollection services,
        int retryCount,
        Func<int, TimeSpan> sleepDurationProvider,
        Action<Exception, TimeSpan>? onRetry = null)
        where TException : Exception
    {
        if (onRetry == null)
            return services
                .AddTransient<RetryPolicy>(_ => Policy
                    .Handle<TException>()
                    .WaitAndRetry(
                        retryCount,
                        sleepDurationProvider)
                );

        return services
            .AddTransient<RetryPolicy>(_ => Policy
                .Handle<TException>()
                .WaitAndRetry(
                    retryCount,
                    sleepDurationProvider,
                    onRetry)
            );
    }
}