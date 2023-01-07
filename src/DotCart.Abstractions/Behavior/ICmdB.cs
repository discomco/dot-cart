using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public interface ICmdB
{
    IID AggregateID { get; }
    string CmdType { get; }
    byte[] Data { get; }
    byte[] MetaData { get; }

}