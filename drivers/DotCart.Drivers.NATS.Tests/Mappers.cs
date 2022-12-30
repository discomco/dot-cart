using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Core;
using DotCart.TestKit;

namespace DotCart.Drivers.NATS.Tests;

public static class Mappers
{
    public static readonly Hope2Cmd<TheBehavior.Cmd, TheContract.Hope> _hope2Cmd =
        hope => TheBehavior.Cmd.New(hope.AggId, hope.Payload,
            EventMeta.New(NameAtt.Get<TheBehavior.IAggregateInfo>(), hope.AggId));
}