namespace DotCart.Context.Abstractions;

public interface IAggregatePolicy
{
    void SetBehavior(IAggregate aggregate);
}