using DotCart.Behavior;
using DotCart.Schema;

namespace DotCart.Effects.Drivers;

public interface IAggregateStoreDriver : IDriver, IClose
{
    Task LoadAsync(IAggregate aggregate);
    Task SaveAsync(IAggregate aggregate);
}

public interface IClose
{
    void Close();
}

public interface IEventStoreDriver : IAggregateStoreDriver
{
    Task<IEnumerable<IEvt>> ReadEventsAsync(IID ID);
    Task<AppendResult> AppendEventsAsync(IID ID, IEnumerable<IEvt> events);
    
}