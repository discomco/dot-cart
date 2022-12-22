using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Spokes;
using DotCart.Core;
using DotCart.Drivers.Default;
using DotCart.Drivers.NATS;
using DotCart.Drivers.Redis;
using Engine.Behavior;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Constants = Engine.Contract.Constants;

namespace Engine.Context;

public static class ChangeDetails
{
    // public class SpokeBuilder : SpokeBuilderT<Spoke>
    // {
    //     public SpokeBuilder(Spoke spoke, IEnumerable<IActor<Spoke>> actors) : base(spoke, actors)
    //     {
    //     }
    // }

    public const string Spoke_v1 = "engine:change_details:spoke:v1";


    public const string ToRedisDoc_v1 = Behavior.ChangeDetails.Topics.Evt_v1 + ":to_redis_doc";

    private static readonly Hope2Cmd<Behavior.ChangeDetails.Cmd, Contract.ChangeDetails.Hope>
        _hope2Cmd =
            hope => Behavior.ChangeDetails.Cmd.New(
                hope.AggId.IDFromIdString(),
                hope.Payload,
                EventMeta.New(
                    NameAtt.Get<IEngineAggregateInfo>(),
                    hope.AggId
                )
            );

    private static readonly Evt2Fact<Contract.ChangeDetails.Fact, Behavior.ChangeDetails.IEvt>
        _evt2Fact =
            evt => Contract.ChangeDetails.Fact.New(
                evt.AggregateId,
                evt.GetPayload<Contract.ChangeDetails.Payload>()
            );

    public static IServiceCollection AddChangeDetailsSpoke(this IServiceCollection services)
    {
        return services
            .AddEngineBehavior()
            .AddChangeDetailsMappers()
            .AddHostedSpokeT<Spoke>()
            .AddTransient<IActor<Spoke>, ToRedisDoc>()
            .AddDefaultDrivers<Schema.Engine, IEngineSubscriptionInfo>()
            .AddSpokedNATSResponder<Spoke, Contract.ChangeDetails.Hope, Behavior.ChangeDetails.Cmd>();
    }

    public static IServiceCollection AddChangeDetailsMappers(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2Fact)
            .AddTransient(_ => _hope2Cmd);
    }

    [Name(Spoke_v1)]
    public class Spoke : SpokeT<Spoke>
    {
        public Spoke(
            IExchange exchange,
            IProjector projector)
            : base(exchange, projector)
        {
        }
    }

    [Name(ToRedisDoc_v1)]
    [DbName(Constants.DocRedisDbName)]
    public class ToRedisDoc : ProjectionT<
        IRedisStore<Schema.Engine>,
        Schema.Engine,
        Behavior.ChangeDetails.IEvt>, IActor<Spoke>
    {
        public ToRedisDoc(IExchange exchange,
            IRedisStore<Schema.Engine> modelStore,
            Evt2State<Schema.Engine, Behavior.ChangeDetails.IEvt> evt2State,
            StateCtorT<Schema.Engine> newDoc) : base(exchange,
            modelStore,
            evt2State,
            newDoc)
        {
        }
    }
}