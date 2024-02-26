using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Core;
using DotCart.Abstractions.Schema;
using DotCart.Behavior;
using DotCart.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.TestUtils;

public static class Start
{
    public static readonly PayloadCtorT<Contract.Start.Payload>
        PayloadCtor =
            () => Contract.Start.Payload.New;

    public static readonly CmdCtorT<
            Contract.Schema.EngineID,
            Contract.Start.Payload,
            MetaB>
        CmdCtor =
            (_, _, _) =>
            {
                var ID = Schema.DocIDCtor();
                return Command.New<Contract.Start.Payload>(
                    ID,
                    PayloadCtor().ToBytes(),
                    Schema.MetaCtor(ID.Id()).ToBytes());
            };

    public static readonly HopeCtorT<Contract.Start.Payload>
        HopeCtor =
            (_, _) => HopeT<Contract.Start.Payload>.New(Schema.DocIDCtor().Id(), PayloadCtor());

    public static readonly FactCtorT<Contract.Start.Payload, MetaB>
        FactCtor =
            (_, _, _) =>
            {
                var ID = Schema.DocIDCtor();
                return FactT<Contract.Start.Payload, MetaB>.New(
                    ID.Id(),
                    PayloadCtor(),
                    Schema.MetaCtor(ID.Id()));
            };

    public static readonly EvtCtorT<Contract.Start.Payload, MetaB>
        EvtCtor =
            (_, _, _) => Behavior.Start._newEvt(
                Schema.DocIDCtor(),
                PayloadCtor().ToBytes(),
                Schema.MetaCtor(null).ToBytes()
            );

    public static readonly StateCtorT<Contract.Schema.Engine>
        DocCtor =
            () => Contract.Schema.Engine.New(
                Schema.DocIDCtor().Id(),
                Contract.Schema.Engine.Flags.Initialized,
                Contract.Schema.Details.New("Engine #32", "An Initialized Engine"),
                Contract.Schema.Rpm.New(0));

    public static IServiceCollection AddTestFuncs(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Schema.DocIDCtor)
            .AddTransient(_ => Schema.MetaCtor)
            .AddTransient(_ => PayloadCtor)
            .AddTransient(_ => FactCtor);
    }
}