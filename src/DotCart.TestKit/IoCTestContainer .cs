using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DotCart.TestKit;

public class IoCTestContainer : IDisposable
{
    public IoCTestContainer()
    {
        Services.AddBaseTestEnv();
    }

    public IServiceCollection Services { get; } = new ServiceCollection();
    private IServiceProvider Provider => Services.BuildServiceProvider();


    public void Dispose()
    {
    }


    public T GetService<T>()
    {
        return Provider.GetService<T>();
    }

    public T GetRequiredService<T>()
    {
        return Provider.GetRequiredService<T>();
    }

    public T GetHostedService<T>()
    {
        var candidates = Provider.GetServices<IHostedService>();
        foreach (var candidate in candidates)
            if (candidate is T cand)
                return cand;
        return default;
    }

    public IEnumerable<T> GetServices<T>()
    {
        return Provider.GetServices<T>();
    }
}