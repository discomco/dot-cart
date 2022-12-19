using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Core;
using DotCart.TestKit.Behavior;
using DotCart.TestKit.Contract;
using DotCart.TestKit.Schema;
using k8s;

namespace DotCart.Drivers.NATS.Tests;

public static class Mappers
{
    public static readonly Hope2Cmd<TheCmd, TheHope> _hope2Cmd =
        hope => TheCmd.New(hope.AggId, hope.Payload, EventMeta.New(NameAtt.Get<ITheAggregateInfo>(), hope.AggId));
}