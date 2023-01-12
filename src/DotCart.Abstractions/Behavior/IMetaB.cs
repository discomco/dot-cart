namespace DotCart.Abstractions.Behavior;

public interface IMetaB
{
    string AggregateId { get; set; }
    string AggregateName { get; set; }
    byte[] Data { get; set; }

    void SetData(byte[] data);
}