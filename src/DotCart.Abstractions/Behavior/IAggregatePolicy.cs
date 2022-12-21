namespace DotCart.Abstractions.Behavior;

public interface IAggregatePolicy
{
    void SetBehavior(IAggregate aggregate);
    string Name { get; }
}