namespace DotCart.Behavior;

public interface IDomainPolicy
{
    void SetBehavior(IAggregate aggregate);
}

public interface IDomainPolicy<in TAggregate> : IDomainPolicy
    where TAggregate : IAggregate
{
}


public abstract class DomainPolicy<TAggregate, TEvt> : IDomainPolicy<TAggregate>
    where TAggregate : IAggregate
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