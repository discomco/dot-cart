using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Behavior;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.TestUtils;

public static class ChangeDetails
{
    public static readonly PayloadCtorT<
            Contract.ChangeDetails.Payload>
        PayloadCtor =
            () => Contract.ChangeDetails.Payload.New(Contract.Schema.Details.New("Engine #2", "A V8 Merlin engine."));

    public static readonly PayloadCtorT<
            Contract.ChangeDetails.Payload>
        NewPayloadCtor =
            () => Contract.ChangeDetails.Payload.New(Contract.Schema.Details.New("Engine #5", "A Wankel Motor."));


    public static readonly HopeCtorT<Contract.ChangeDetails.Payload>
        HopeCtor =
            (_, _) => HopeT<Contract.ChangeDetails.Payload>.New(Schema.DocIDCtor().Id(), PayloadCtor());

    public static readonly FactCtorT<Contract.ChangeDetails.Payload, MetaB>
        FactCtor =
            (_, _, _) =>
            {
                var ID = Schema.DocIDCtor();
                return FactT<Contract.ChangeDetails.Payload, MetaB>.New(
                    ID.Id(),
                    PayloadCtor(),
                    Schema.MetaCtor(ID.Id()));
            };


    public static readonly CmdCtorT<Contract.Schema.EngineID, Contract.ChangeDetails.Payload, MetaB>
        CmdCtor =
            (_, _, _) => Command.New<Contract.ChangeDetails.Payload>(
                Schema.DocIDCtor(),
                PayloadCtor().ToBytes(),
                MetaB.New(NameAtt.Get<IEngineAggregateInfo>(), Schema.DocIDCtor().Id()).ToBytes()
            );


    public static readonly EvtCtorT<Contract.ChangeDetails.Payload, MetaB>
        EvtCtor =
            (_, _, _) => Behavior.ChangeDetails._newEvt(
                Schema.DocIDCtor(),
                PayloadCtor().ToBytes(),
                Schema.MetaCtor(null).ToBytes());


    [Tag(StateTags.Invalid)] public static readonly StateCtorT<Contract.Schema.Engine>
        InvalidEngineCtor =
            () => Contract.Schema.Engine.New(
                Schema.DocIDCtor().Id(),
                Contract.Schema.EngineStatus.Unknown,
                Contract.Schema.Details.New("Invalid Test Engine",
                    $"This is an INVALID Engine for ChangeDetails because state is [{Contract.Schema.EngineStatus.Unknown}]"),
                Contract.Schema.Rpm.New(0));

    [Tag(StateTags.Valid)] public static readonly StateCtorT<Contract.Schema.Engine>
        ValidEngineCtor =
            () => Contract.Schema.Engine.New(
                Schema.DocIDCtor().Id(),
                Contract.Schema.EngineStatus.Initialized,
                Contract.Schema.Details.New("Valid Test Engine",
                    $"This is an VALID Engine for ChangeDetails because state is [{Contract.Schema.EngineStatus.Initialized}] "),
                Contract.Schema.Rpm.New(0));


    public static IServiceCollection AddTestFuncs(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => FactCtor)
            .AddTransient(_ => PayloadCtor)
            .AddTransient(_ => Schema.MetaCtor)
            .AddTransient(_ => Schema.DocIDCtor);
    }
}