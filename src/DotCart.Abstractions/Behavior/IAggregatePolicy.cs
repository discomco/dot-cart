namespace DotCart.Abstractions.Behavior;

public interface IAggregatePolicy
{
    string Name { get; }
    void SetBehavior(IAggregate aggregate);
}