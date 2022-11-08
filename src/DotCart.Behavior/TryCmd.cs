using DotCart.Contract;


namespace DotCart.Behavior;

public abstract class TryCmd<TCmd> : ITry<TCmd>
    where TCmd : ICmd 
{
    protected IAggregate Aggregate;

    public string CmdType => Topic.Get<TCmd>();

    public void SetAggregate(IAggregate aggregate)
    {
        Aggregate = aggregate;
    }
    public abstract IFeedback Verify(TCmd cmd);
    public abstract IEnumerable<Event> Raise(TCmd cmd);
}

