using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace DockTrace.TestKit;

public static class Inject
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


// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.DependencyInjection.Extensions;
// using Microsoft.Extensions.Hosting;
// using Serilog;

// namespace DotCart.TestKit;

// public static partial class Inject
// {
//     public static IServiceCollection AddHostExecutor(this IServiceCollection services)
//     {
//         services.TryAddSingleton<IHostExecutor, HostExecutor>();
//         return services;
//     }
// }

// public interface IHostExecutor
// {
//     IEnumerable<IHostedService> Services { get; }
//     Task StartAsync(CancellationToken token = default);
//     Task StopAsync(CancellationToken token = default);
// }

// public class HostExecutor : IHostExecutor
// {
//     private readonly ILogger _logger;

//     public HostExecutor(ILogger logger, IEnumerable<IHostedService> services)
//     {
//         _logger = logger;
//         Services = services;
//     }

//     public IEnumerable<IHostedService> Services { get; }

//     public Task StartAsync(CancellationToken token)
//     {
//         try
//         {
//             ExecuteAsync(service => service.StartAsync(token));
//         }
//         catch (Exception ex)
//         {
//             _logger?.Error("An error occurred starting the application", ex);
//             throw;
//         }

//         return Task.CompletedTask;
//     }

//     public Task StopAsync(CancellationToken token)
//     {
//         try
//         {
//             ExecuteAsync(service => service.StopAsync(token));
//         }
//         catch (Exception ex)
//         {
//             _logger?.Error("An error occurred stopping the application", ex);
//         }

//         return Task.CompletedTask;
//     }

//     private Task ExecuteAsync(Func<IHostedService, Task> callback)
//     {
//         List<Exception> exceptions = null;

//         foreach (var service in Services)
//             try
//             {
//                 callback(service);
//             }
//             catch (Exception ex)
//             {
//                 exceptions ??= new List<Exception>();

//                 exceptions.Add(ex);
//             }

//         // Throw an aggregate exception if there were any exceptions
//         if (exceptions != null) throw new AggregateException(exceptions);
//         return Task.CompletedTask;
//     }
// }