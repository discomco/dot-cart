using DotCart.Behavior;

namespace DotCart.Effects;

public interface IAggregateStore : IClose
{
    void Load(IAggregate aggregate);
    void Save(IAggregate aggregate);
}

public interface IClose
{
    void Close();
}

public interface IEventStore : IAggregateStore
{
}

