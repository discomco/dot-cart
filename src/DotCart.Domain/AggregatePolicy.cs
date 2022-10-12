
namespace DotCart.Domain;


public interface IAggregatePolicy
{
    void SetAggregate(IAggregate aggregate);
}

public interface IAggregatePolicy<in TAggregate> : IAggregatePolicy
    where TAggregate : IAggregate
{
    
}

public abstract class AggregatePolicy<TAggregate, TEvt> : IAggregatePolicy<TAggregate>
    where TAggregate : IAggregate
{
    protected IAggregate Aggregate;

    protected AggregatePolicy
    (
        string topic,
        ITopicPubSub pubSub
    )
    {
        pubSub.Subscribe(topic, Enforce);
    }

    protected abstract Task Enforce(IEvt evt);


    public void SetAggregate(IAggregate aggregate) 
    {
        Aggregate = aggregate;
    }
}