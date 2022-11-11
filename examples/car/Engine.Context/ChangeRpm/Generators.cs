using DotCart.Client;
using Engine.Client.ChangeRpm;
using Engine.Client.Schema;

namespace Engine.Context.ChangeRpm;

public static class Generators
{
    public static GenerateHope<Hope> _generateHope => () =>
    {
        var engineID = EngineID.New();
        var pl = Payload.New(Random.Shared.Next(20));
        return Hope.New(engineID.Id(), pl);
    };
}