namespace DotCart.Abstractions.Behavior;

public interface IAggregatePolicy
{
    string Name { get; }
    void SetAggregate(IAggregate aggregate);
    string Topic { get; }
    Task HandleEvtAsync(IEvtB evt, CancellationToken cancellationToken = default);
}