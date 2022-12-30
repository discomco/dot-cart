using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.TestKit;

public static class TheBehavior
{
    [Name("the:aggregate")]
    public interface IAggregateInfo : IAggregateInfoB
    {
    }

    [Topic(TheConstants.CmdTopic)]
    public record Cmd(IID AggregateID, TheContract.Payload Payload, EventMeta Meta)
        : CmdT<TheContract.Payload, EventMeta>(AggregateID, Payload, Meta)
    {
        public static Cmd New(string aggId, TheContract.Payload payload, EventMeta meta)
        {
            return new Cmd(aggId.IDFromIdString(), payload, meta);
        }
    }

    [Topic(TheConstants.EvtTopic)]
    public interface IEvt : IEvtT<TheContract.Payload>
    {
    }
}