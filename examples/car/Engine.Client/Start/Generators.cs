using DotCart.Client;
using DotCart.Core;
using Engine.Client.Schema;

namespace Engine.Client.Start;

public static class Generators
{
    public static GenerateHope<Hope> _generateHope => () =>
    {
        var pl = Payload.New;
        var aggId = EngineID.New();
        return Hope.New(aggId.Id(), pl.ToBytes());
    };
}