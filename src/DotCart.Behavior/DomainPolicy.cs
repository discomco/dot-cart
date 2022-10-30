namespace DotCart.Behavior;

public interface IDomainPolicy
{
    void SetBehavior(IAggregate aggregate);
}


public abstract class DomainPolicy<TEvt> : IDomainPolicy
{
    protected IAggregate? Aggregate;

    protected DomainPolicy
    (
        string topic,
        ITopicPubSub pubSub
    )
    {
        pubSub.Subscribe(topic, Enforce);
    }


    public void SetBehavior(IAggregate aggregate)
    {
        Aggregate = aggregate;
    }

    protected abstract Task Enforce(IEvt evt);
}