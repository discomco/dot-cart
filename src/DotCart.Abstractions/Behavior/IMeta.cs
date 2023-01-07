namespace DotCart.Abstractions.Behavior;

public interface IMeta
{
    string AggregateId { get; set; }
    string AggregateName { get; set; }
    byte[]? Data { get; set; }
}