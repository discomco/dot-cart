using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Contract;

namespace Engine.Context.Initialize;

public static class Generators
{
    public static readonly GenerateHope<Contract.Initialize.Hope> _genHope =
        () =>
        {
            var details = Schema.Details.New("NewEngine");
            var aggID = Schema.EngineID.New();
            var pl = Contract.Initialize.Payload.New(details);
            return Contract.Initialize.Hope.New(aggID.Id(), pl.ToBytes());
        };
}