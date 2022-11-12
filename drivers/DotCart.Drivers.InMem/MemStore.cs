using System.Collections.Immutable;
using DotCart.Context.Effects.Drivers;
using DotCart.Contract.Schemas;
using Microsoft.Extensions.DependencyInjection;
using static System.GC;

namespace DotCart.Drivers.InMem;

// TODO Inspect CancellationToken

public static partial class Inject
{
    public static IServiceCollection AddMemStore<TState>(this IServiceCollection services) 
        where TState : IState
    {
        return services
            .AddSingleton<IModelStore<TState>, MemStore<TState>>();
    }
}




public class MemStore<TState> : IModelStore<TState> where TState : IState
{
    private readonly object delMutex = new();
    private readonly object existMutex = new();
    private readonly object getMutex = new();

    private readonly object setMutex = new();
    private IImmutableDictionary<string, TState> InnerStore = ImmutableSortedDictionary<string, TState>.Empty;

    public void Dispose()
    {
        InnerStore.Clear();
        SuppressFinalize(this);
    }

    public Task<TState> SetAsync(string id, TState entity, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            lock (setMutex)
            {
                InnerStore = InnerStore.SetItem(id, entity);
                return entity;
            }
        }, cancellationToken);
    }

    public Task<TState> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        return Task.Run<TState>(() =>
        {
            lock (delMutex)
            {
                if (!Exists(id, cancellationToken).Result) return default;
                var res = InnerStore.GetValueOrDefault(id);
                InnerStore = InnerStore.Remove(id);
                return res ?? default;
            }
        }, cancellationToken);
    }

    public Task<bool> Exists(string id, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            lock (existMutex)
            {
                return InnerStore.Keys.Contains(id);
            }
        }, cancellationToken);
    }

    public Task<TState?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            lock (getMutex)
            {
                return !Exists(id).Result
                    ? default
                    : InnerStore[id];
            }
        });
    }

    public async Task<bool> HasData(CancellationToken cancellationToken)
    {
        return InnerStore.Count > 0;
    }

    public void Close()
    {
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    public Task CloseAsync(bool allowCommandsToComplete)
    {
        return Task.CompletedTask;
    }
}