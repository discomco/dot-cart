namespace DotCart.Abstractions.Behavior;

public interface IAggregatePolicy
{
    string Name { get; }
    string Topic { get; }
    void SetAggregate(IAggregate aggregate);
    Task HandleEvtAsync(IEvtB evt, CancellationToken cancellationToken = default);
}