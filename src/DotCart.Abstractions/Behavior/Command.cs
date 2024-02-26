using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public record Command(
    IID AggregateID,
    string CmdType,
    byte[] Data,
    byte[] MetaData) : ICmdB
{
    public string CmdType { get; } = CmdType;

    public static Command New<TPayload>(IID id, byte[] data, byte[] meta)
        where TPayload : IPayload
    {
        return new Command(
            id,
            CmdTopicAtt.Get<TPayload>(),
            data,
            meta);
    }
}