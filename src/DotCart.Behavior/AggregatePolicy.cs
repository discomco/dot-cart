namespace DotCart.Behavior;

public interface IDomainPolicy
{
    void SetBehavior(IAggregate aggregate);
}

public interface IDomainPolicy<in TAggregate> : IDomainPolicy
    where TAggregate : IAggregate
{
}