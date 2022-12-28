using DotCart.Abstractions.Actors;
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


    public T Resolve<T>()
    {
        return Provider.GetService<T>();
    }

    public T ResolveRequired<T>()
    {
        return Provider.GetRequiredService<T>();
    }

    public T ResolveHosted<T>()
    {
        var candidates = Provider.GetServices<IHostedService>();
        foreach (var candidate in candidates)
            if (candidate is T cand)
                return cand;
        return default;
    }

    public IEnumerable<T> ResolveAll<T>()
    {
        return Provider.GetServices<T>();
    }

    public IActor<TSpoke>? ResolveActor<TSpoke, TActor>()
        where TSpoke : ISpokeT<TSpoke>
        where TActor : IActor<TSpoke>
    {
        IEnumerable<IActor<TSpoke>?> actors = ResolveAll<IActor<TSpoke>>();
        var res = actors.FirstOrDefault(actor => actor.GetType() == typeof(TActor));
        if (res == null)
            throw new InvalidOperationException("Actor of type [" + typeof(TActor) +
                                                "] was not found. Please register it.");
        return res;
    }
}