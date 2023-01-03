using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;

namespace DotCart.Drivers.NATS.Tests;

public static class Mappers
{
    public static readonly Hope2Cmd<TheContract.Payload, TheContract.Meta> _hope2Cmd =
        hope => CmdT<TheContract.Payload, TheContract.Meta>.New(
            hope.AggId.IDFromIdString(),
            hope.Payload,
            TheContract.Meta.New(NameAtt.Get<TheBehavior.IAggregateInfo>(), hope.AggId)
        );
}