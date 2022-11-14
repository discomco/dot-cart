using DotCart.Context.Abstractions;
using DotCart.Context.Abstractions.Drivers;
using DotCart.Context.Effects;
using DotCart.Contract.Schemas;
using DotCart.Core;
using DotCart.Drivers.InMem;
using DotCart.Drivers.Redis;
using Engine.Contract.Initialize;
using Engine.Contract.Schema;

namespace Engine.Context.Initialize;

public static class Mappers
{
    public static readonly Evt2Fact<Fact, IEvt> _evt2Fact =
        evt => Fact.New(evt.AggregateID.Id(), evt.GetPayload<Payload>());

    public static readonly Hope2Cmd<Cmd, Hope> _hope2Cmd =
        hope => Cmd.New(hope.AggId.IDFromIdString(), hope.GetPayload<Payload>());

    public static Evt2State<Common.Schema.Engine, IEvt> _evt2State => (state, evt) =>
    {
        if (evt == null) return state;
        if (evt.GetPayload<Common.Schema.Engine>() == null) return state;
        state = evt.GetPayload<Common.Schema.Engine>();
        state.Id = evt.AggregateID.Id();
        state.Status = EngineStatus.Initialized;
        return state;
    };
}

public static class Effects
{
    public interface IResponder : IActor<Spoke>
    {
    }

    [Name("Engine.Initialize.Responder")]
    public class Responder : ResponderT<Spoke, MemResponderDriver<Hope>, Hope, Cmd>, IResponder
    {
        public Responder(IExchange exchange,
            IResponderDriver<Hope> responderDriver,
            ICmdHandler cmdHandler,
            Hope2Cmd<Cmd, Hope> hope2Cmd) : base(exchange,
            responderDriver,
            cmdHandler,
            hope2Cmd)
        {
        }
    }

    public interface IToMemDoc
    {
    }

    [Name("Engine.Initialize.ToMemDocProjection")]
    public class ToMemDoc : ProjectionT<Spoke, MemStore<Common.Schema.Engine>, Common.Schema.Engine, IEvt>,
        IToMemDoc
    {
        public ToMemDoc(IExchange exchange,
            IModelStore<Common.Schema.Engine> modelStore,
            Evt2State<Common.Schema.Engine, IEvt> evt2State) : base(exchange,
            modelStore,
            evt2State)
        {
        }
    }

    public interface IToRedisDoc : IProjection<RedisStore<Common.Schema.Engine>, Common.Schema.Engine, IEvt>, IConsumer
    {
    }

    public class ToRedisDoc : ProjectionT<Spoke, RedisStore<Common.Schema.Engine>, Common.Schema.Engine, IEvt>,
        IToRedisDoc
    {
        public ToRedisDoc(IExchange exchange,
            IModelStore<Common.Schema.Engine> modelStore,
            Evt2State<Common.Schema.Engine, IEvt> evt2State) : base(exchange,
            modelStore,
            evt2State)
        {
        }
    }
}