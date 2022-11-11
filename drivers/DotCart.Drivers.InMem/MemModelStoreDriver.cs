using System.Collections.Immutable;
using DotCart.Context.Effects.Drivers;
using DotCart.Context.Schemas;

namespace DotCart.Drivers.InMem;

public abstract class MemModelStoreDriver<TState> : IModelStoreDriver<TState> where TState : IState
{
    private readonly object delMutex = new();
    private readonly object existMutex = new();
    private readonly object getMutex = new();

    private readonly object setMutex = new();
    private IImmutableDictionary<string, TState> InnerStore = ImmutableSortedDictionary<string, TState>.Empty;

    public void Dispose()
    {
        InnerStore.Clear();
        GC.SuppressFinalize(this);
    }

    public Task<TState> SetAsync(string id, TState entity)
    {
        return Task.Run(() =>
        {
            lock (setMutex)
            {
                InnerStore = InnerStore.SetItem(id, entity);
                return entity;
            }
        });
    }

    public Task<bool> DeleteAsync(string id)
    {
        return Task.Run(() =>
        {
            lock (delMutex)
            {
                if (!Exists(id).Result) return false;
                InnerStore = InnerStore.Remove(id);
                return true;
            }
        });
    }

    public Task<bool> Exists(string id)
    {
        return Task.Run(() =>
            {
                lock (existMutex)
                {
                    return InnerStore.Keys.Contains(id);
                }
            }
        );
    }

    public Task<TState?> GetByIdAsync(string id)
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

    public async Task<bool> HasData()
    {
        return InnerStore.Count > 0;
    }

    public void Close()
    {
    }
}