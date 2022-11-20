using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Contract.Initialize;
using Engine.Contract.Schema;

namespace Engine.Context.Initialize;

public static class Generators
{
    public static readonly GenerateHope<Hope> _genHope =
        () =>
        {
            var details = Details.New("NewEngine");
            var aggID = EngineID.New();
            var pl = Payload.New(details);
            return Hope.New(aggID.Id(), pl.ToBytes());
        };
}