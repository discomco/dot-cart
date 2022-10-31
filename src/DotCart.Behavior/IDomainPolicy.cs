namespace DotCart.Behavior;

public interface IDomainPolicy
{
    void SetBehavior(IAggregate aggregate);
}