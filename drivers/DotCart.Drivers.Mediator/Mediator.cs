using DotCart.Abstractions.Actors;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Drivers.Mediator;

public static partial class Inject
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        return services
            .AddSingleton<IMediator, Mediator>();
    }
}

internal class Mediator : IMediator
{
    private static readonly AsyncLocal<Dictionary<Type, List<object>>> handlers =
        new();

    public Dictionary<Type, List<object>> Handlers =>
        handlers.Value ?? (handlers.Value = new Dictionary<Type, List<object>>());

    public async Task PublishAsync<T>(T publishedEvent)
    {
        foreach (var handler in GetHandlersOf<T>())
            try
            {
                switch (handler)
                {
                    case Action<T> action:
                        action(publishedEvent);
                        break;
                    case Func<T, Task> action:
                        await action(publishedEvent);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
    }

    public void Subscribe<T>(Action<T> handler)
    {
        GetHandlersOf<T>().Add(handler);
    }

    public void Subscribe<T>(Func<T, Task> handler)
    {
        GetHandlersOf<T>().Add(handler);
    }

    public void Dispose()
    {
        foreach (var handlersOfT in Handlers.Values) handlersOfT.Clear();
        Handlers.Clear();
    }

    private ICollection<object> GetHandlersOf<T>()
    {
        return Handlers.GetValueOrDefault(typeof(T)) ?? (Handlers[typeof(T)] = new List<object>());
    }
}