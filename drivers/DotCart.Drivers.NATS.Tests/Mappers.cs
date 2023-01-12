using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit.Mocks;

namespace DotCart.Drivers.NATS.Tests;

public static class Mappers
{
    public static readonly Hope2Cmd<TheContract.Payload, TheContract.Meta> _hope2Cmd =
        hope => Command.New<TheContract.Payload>(
            hope.AggId.IDFromIdString(),
            hope.Payload.ToBytes(),
            TheContract.Meta.New(NameAtt.Get<TheBehavior.IAggregateInfo>(), hope.AggId).ToBytes()
        );
}