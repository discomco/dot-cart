using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace DotCart.TestKit;

public static partial class Inject
{
    public static IServiceCollection AddHostExecutor(this IServiceCollection services)
    {
        services.TryAddSingleton<IHostExecutor, HostExecutor>();
        return services;
    }
}

public interface IHostExecutor
{
    IEnumerable<IHostedService> Services { get; }
    Task StartAsync(CancellationToken token = default);
    Task StopAsync(CancellationToken token = default);
}

public class HostExecutor : IHostExecutor
{

    public HostExecutor(IEnumerable<IHostedService> services)
    {
        Services = services;
    }

    public IEnumerable<IHostedService> Services { get; }

    public Task StartAsync(CancellationToken token)
    {
        return ExecuteAsync(service => service.StartAsync(token), token);
    }

    public Task StopAsync(CancellationToken token)
    {
        return ExecuteAsync(service => service.StopAsync(token), token);
    }

    private Task ExecuteAsync(Func<IHostedService, Task> callback, CancellationToken cancellationToken = default)
    {
        List<Exception>? exceptions = null;
        foreach (var service in Services)
            try
            {
                callback(service);
            }
            catch (Exception ex)
            {
                exceptions ??= new List<Exception>();
                exceptions.Add(ex);
            }

        // Throw an aggregate exception if there were any exceptions
        if (exceptions != null)
            throw new AggregateException(exceptions);
        return Task.CompletedTask;
    }
}
