namespace DotCart.Behavior;

public interface IAggregateStore : IClose
{
    Task LoadAsync(IAggregate aggregate);
    Task SaveAsync(IAggregate aggregate);
}

public interface IClose
{
    void Close();
}

public interface IEventStore : IAggregateStore
{
}