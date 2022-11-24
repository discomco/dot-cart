using DotCart.Abstractions.Schema;
using Engine.Contract;

namespace Engine.Utils;

public static class ChangeRpm
{
    public static GenerateHope<Contract.ChangeRpm.Hope> _generateHope => () =>
    {
        var engineID = Schema.IDCtor();
        var pl = Contract.ChangeRpm.Payload.New(Random.Shared.Next(20));
        return Contract.ChangeRpm.Hope.New(engineID.Id(), pl);
    };
}