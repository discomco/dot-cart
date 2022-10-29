namespace DotCart.Behavior;

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

    protected abstract Task Enforce(IEvt evt);


    public void SetBehavior(IAggregate aggregate) 
    {
        Aggregate = aggregate;
    }
}