using DotCart.Contract;
using DotCart.Core;
using Engine.Contract.Schema;

namespace Engine.Contract.Start;

public static class Generators
{
    public static GenerateHope<Hope> _generateHope => () =>
    {
        var pl = Payload.New;
        var aggId = EngineID.New();
        return Hope.New(aggId.Id(), pl.ToBytes());
    };
}