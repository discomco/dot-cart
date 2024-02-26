using DotCart.Abstractions;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Core;
using DotCart.Abstractions.Schema;
using DotCart.Behavior;
using DotCart.Schema;
using Engine.Behavior;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.TestUtils;

public static class ChangeRpm
{
    public static readonly CmdCtorT<Contract.Schema.EngineID, Contract.ChangeRpm.Payload, MetaB>
        CmdCtor =
            (id, _, _) => Command.New<Contract.ChangeRpm.Payload>(
                id,
                PayloadCtor().ToBytes(),
                MetaB.New(NameAtt.Get<IEngineAggregateInfo>(), id.Id()).ToBytes());

    public static readonly PayloadCtorT<Contract.ChangeRpm.Payload>
        PayloadCtor =
            () => Contract.ChangeRpm.Payload.New(Random.Shared.Next(-10, +10));

    public static readonly HopeCtorT<Contract.ChangeRpm.Payload>
        HopeCtor =
            (_, _) => HopeT<Contract.ChangeRpm.Payload>.New(Schema.DocIDCtor().Id(), PayloadCtor());

    public static readonly FactCtorT<Contract.ChangeRpm.Payload, MetaB>
        FactCtor =
            (_, _, _) =>
            {
                var ID = Schema.DocIDCtor();
                return FactT<Contract.ChangeRpm.Payload, MetaB>.New(
                    ID.Id(),
                    PayloadCtor(),
                    Schema.MetaCtor(ID.Id()));
            };

    public static readonly EvtCtorT<Contract.ChangeRpm.Payload, MetaB>
        EvtCtor =
            (_, _, _) =>
            {
                var ID = Schema.DocIDCtor();
                return Behavior.ChangeRpm._newEvt(
                    ID,
                    PayloadCtor().ToBytes(),
                    Schema.MetaCtor(ID.Id()).ToBytes()
                );
            };

    public static IServiceCollection AddTestFuncs(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Schema.DocIDCtor)
            .AddTransient(_ => Schema.MetaCtor)
            .AddTransient(_ => PayloadCtor)
            .AddTransient(_ => FactCtor);
    }
}