using DotCart.Behavior;

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
}