using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
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

public static class ChangeRpm
{
    public const string ToRedisDoc_v1 = Behavior.ChangeRpm.Topics.Evt_v1 + ":to_redis_doc";


    public const string Spoke_v1 = "engine:change_rpm:spoke:v1";

    public static IServiceCollection AddChangeRpmSpoke(this IServiceCollection services)
    {
        return services
            .AddEngineBehavior()
            .AddChangeRpmMappers()
            .AddHostedSpokeT<Spoke>()
            .AddDefaultDrivers<Schema.Engine, IEngineSubscriptionInfo>()
            .AddTransient<IActor<Spoke>, ToRedisDoc>()
            .AddSpokedNATSResponder<Spoke, Contract.ChangeRpm.Hope, Behavior.ChangeRpm.Cmd>();
    }


    [Name(ToRedisDoc_v1)]
    [DbName(Constants.DocRedisDbName)]
    public class ToRedisDoc : ProjectionT<
        IRedisStore<Schema.Engine>,
        Schema.Engine,
        Behavior.ChangeRpm.IEvt>, IActor<Spoke>
    {
        public ToRedisDoc(IExchange exchange,
            IRedisStore<Schema.Engine> modelStore,
            Evt2State<Schema.Engine, Behavior.ChangeRpm.IEvt> evt2Doc,
            StateCtorT<Schema.Engine> newDoc)
            : base(exchange, modelStore, evt2Doc, newDoc)
        {
        }
    }


    [Name(Spoke_v1)]
    public class Spoke : SpokeT<Spoke>
    {
        public Spoke(
            IExchange exchange,
            IProjector projector) : base(exchange, projector)
        {
        }
    }
}